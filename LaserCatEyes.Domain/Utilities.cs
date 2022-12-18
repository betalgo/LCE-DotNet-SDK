using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LaserCatEyes.Domain.Models;

namespace LaserCatEyes.Domain
{
    public static class Utilities
    {
        public static void Forget(this Task task)
        {
            // note: this code is inspired by a tweet from Ben Adams: https://twitter.com/ben_a_adams/status/1045060828700037125
            // Only care about tasks that may fault (not completed) or are faulted,
            // so fast-path for SuccessfullyCompleted and Canceled tasks.
            if (!task.IsCompleted || task.IsFaulted)
            {
                // use "_" (Discard operation) to remove the warning IDE0058: Because this call is not awaited, execution of the current method continues before the call is completed
                // https://docs.microsoft.com/en-us/dotnet/csharp/discards#a-standalone-discard
                _ = ForgetAwaited(task);
            }

            // Allocate the async/await state machine only when needed for performance reason.
            // More info about the state machine: https://blogs.msdn.microsoft.com/seteplia/2017/11/30/dissecting-the-async-methods-in-c/
            async Task ForgetAwaited(Task taskToForget)
            {
                try
                {
                    // No need to resume on the original SynchronizationContext, so use ConfigureAwait(false)
                    await taskToForget.ConfigureAwait(false);
                }
                catch
                {
                    // Nothing to do here
                }
            }
        }

        public static MethodType HttpMethodStringToEnumConverter(string method)
        {
            switch (method.ToUpper())
            {
                case "GET":
                    return MethodType.GET;
                case "POST":
                    return MethodType.POST;
                case "PUT":
                    return MethodType.PUT;
                case "DELETE":
                    return MethodType.DELETE;
                case "HEAD":
                    return MethodType.HEAD;
                case "OPTIONS":
                    return MethodType.OPTIONS;
                case "PATCH":
                    return MethodType.PATCH;
                case "TRACE":
                    return MethodType.TRACE;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        public static async Task<string> ReadBodyStream(Stream body)
        {
            if (!body.CanRead)
            {
                return null;
            }

            if (body.Length == 0)
            {
                return null;
            }

            string bodyString;
            using (var reader = new StreamReader(body))
            {
                body.Position = 0;
                bodyString = await reader.ReadToEndAsync().ConfigureAwait(false);
                body.Position = 0;
            }

            return bodyString;
        }

        public static Guid ToGuid(string src)
        {
            var stringBytes = Encoding.UTF8.GetBytes(src);
            var hashedBytes = new SHA1CryptoServiceProvider()
                .ComputeHash(stringBytes);
            Array.Resize(ref hashedBytes, 16);
            return new Guid(hashedBytes);
        }
    }
}