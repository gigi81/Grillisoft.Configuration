﻿using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Grillisoft.Configuration
{
    internal class DoltRunner
    {
        private readonly string _executable;
        private readonly string _workFolder;
        private readonly Action<string> _output;
        private readonly Action<string> _error;

        public DoltRunner(string workFolder, Action<string> output = null, Action<string> error = null)
        {
            _executable = "dolt";
            _workFolder = workFolder;
            _output = output;
            _error = error;
        }

        public DoltRunner Clone(string repo, string branch = null)
        {
            if (String.IsNullOrEmpty(repo))
                throw new ArgumentNullException(nameof(repo));

            var index = repo.LastIndexOf('/');
            if (index <= 0 || index >= repo.Length - 1)
                throw new ArgumentException($"Invalid dolt repo name {repo}", nameof(repo));

            if (String.IsNullOrEmpty(branch))
                this.RunOrThrow("clone", repo);
            else
                this.RunOrThrow("clone", repo, "-b", branch);

            return new DoltRunner(Path.Combine(_workFolder, repo.Substring(index + 1)), _output, _error);
        }

        public IList<DoltTable> ListTables()
        {
            var ret = new List<DoltTable>();

            new DoltRunner(_workFolder, row =>
            {
                if (DoltTable.IsMatch(row))
                    ret.Add(new DoltTable(row));

                _output?.Invoke(row);
            }, _error).RunOrThrow("ls", "-v");

            return ret;
        }

        public IList<T> Export<T>(string table, ClassMap map = null)
        {
            var path = Path.GetTempFileName();

            try
            {
                this.RunOrThrow("table", "export", "-f", "-file-type", "csv", table, path);

                using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    if(map != null)
                        csv.Configuration.RegisterClassMap(map);

                    return csv.GetRecords<T>().ToList();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        public virtual void RunOrThrow(params string[] args)
        {
            var ret = this.Run(args);
            if (ret != 0)
                throw new Exception($"dolt ({JoinArgs(args)}) exited with code {ret}");
        }

        public virtual int Run(params string[] args)
        {
            return RunProcess(CreateProcess(args)).ExitCode;
        }

        protected virtual Process CreateProcess(string[] args)
        {
            var info = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                FileName = _executable,
                Arguments = JoinArgs(args),
                WorkingDirectory = _workFolder
            };

            var process = new Process();
            process.StartInfo = info;
            process.OutputDataReceived += Process_OutputDataReceived;
            process.ErrorDataReceived += Process_ErrorDataReceived;
            return process;
        }

        private static string JoinArgs(string[] args)
        {
            return String.Join(" ", args.Select(a => a.Contains(' ') ? $"\"{a}\"" : a));
        }

        protected virtual Process RunProcess(Process process)
        {
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            return process;
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(e.Data))
                _output?.Invoke(e.Data);
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(e.Data))
                _error?.Invoke(e.Data);
        }
    }
}
