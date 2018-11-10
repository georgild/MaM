using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Transcoder.Contracts;

namespace Transcoder.Implementations {
    public class FFMPEGTranscoder : ITranscoder {
        public Task<Stream> ExtractImage(string source, uint width, uint height, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public double GetProgress() {
            throw new NotImplementedException();
        }

        public ETranscoderState GetState() {
            throw new NotImplementedException();
        }

        public bool Transcode(string source, string destination, string format) {

            string encoderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"ThirdParty\ffmpeg\bin\ffmpeg.exe");

            ProcessStartInfo oInfo = new ProcessStartInfo(encoderPath) {
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            string args = "-y -i \"{0}\" -f \"{1}\" \"{2}\"";

            oInfo.Arguments = string.Format(args, source, format, destination).Trim();

            Process process = Process.Start(oInfo);
            StreamReader reader = process.StandardOutput;
            string output = reader.ReadToEnd();

            // Write the redirected output to this application's window.
            Console.WriteLine(output);

            process.WaitForExit();

            bool succeeded = process.ExitCode != -1;
            process.Close();
            process.Dispose();

            if (!succeeded) {
                Console.WriteLine(string.Format("ffmpeg video resize failed for {0}", source));
            }

            return succeeded;

        }

        public string QualityCheck(string source) {

            string encoderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"ThirdParty\ffmpeg\bin\ffmpeg.exe");

            ProcessStartInfo oInfo = new ProcessStartInfo(encoderPath) {
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            //string args = "-y -i \"{0}\" -vf blackdetect=d=0.1:pix_th=.1 -f rawvideo NUL";
            string args = "-v error -i \"{0}\" -f null -";

            oInfo.Arguments = string.Format(args, source).Trim();

            Process process = new Process {
                StartInfo = oInfo
            };
            //process.OutputDataReceived += (sender, procArgs) => Console.WriteLine("received output: {0}", procArgs.Data);

            process.Start();
            //process.BeginOutputReadLine();

            //StreamReader reader = process.StandardOutput;

            //var memoryStream = new MemoryStream();
            //await process.StandardOutput.BaseStream.CopyToAsync(memoryStream);
            string output = string.Empty; //  process.StandardOutput.ReadToEnd();

            //Console.WriteLine(output);
            
            process.WaitForExit();
            var bla = Console.ReadLine();
            bool succeeded = process.ExitCode != -1;
            process.Close();
            process.Dispose();

            if (!succeeded) {
                Console.WriteLine(string.Format("ffmpeg video resize failed for {0}", source));
                throw new InvalidOperationException();
            }

            if (output.IndexOf("blackdetect") >= 0) {
                return output.Substring(output.IndexOf("blackdetect"), output.Length);
            }

            return string.Empty;

        }
    }
}
