using System;
using System.Collections.Concurrent;
using NUnit.Framework;

namespace Reincheck.Tests
{
    public abstract class FixtureBase
    {
        readonly ConcurrentStack<IDisposable> _stuffToDispose = new ConcurrentStack<IDisposable>();

        protected const string ValidKey =
            @"PFJTQUtleVZhbHVlPjxNb2R1bHVzPm45Sk91YzNHRFh5dktKZnZVdnNJQjdZQWNOZGtyL3VGZUhnWkFLR0RESmdMTjllanpQWkQyN203UFRMS2dNSzNKN1g0VGl2OWlhYWhvRkNkUjJIZUhWVW0wQTZGWEp2bG1xcUY3cllpY0FoL1Z2Vm5md1JaUnp4YWQvWndtNG56YjBjdVh3UzJPOWtBNTZnWlR0WjA1cTU5L3MrQUVjWGNWUU1wTUR1dGd1bTh3cElkNXovSDFwUUNvVVpOblJpL1V0Qlc0YWhML3dFdU1nQkkvU2t2Mm9PanJ4MlY5Sk16OHFrWmRYSzVHVlIxVjJHMTVYMzdoR3pKU1NhTDdiRWcyQzFzeDZySk9NQWx6OXRVOWZ5dGNqeTJxTGthMmFyeVBGSzdVK2FuUG5RQVQyRWJOMHlQNU14ckcra25PRnZ4YnVZUG9vVnh0c1drZlB3N3JtMEFEUT09PC9Nb2R1bHVzPjxFeHBvbmVudD5BUUFCPC9FeHBvbmVudD48UD4yVHhheVQxbGhObEtRdVFRQVA4blh0UjA5NHJFOUVWOU1KWlNYOStpOUNtakE4N0NYQnk4ZTRsRGg1ZWVibkVmcnV0UWFkMFBGelRIeU5YTUZJWW5QczhYQXM1NFhRckZLNE1GaHA0YmhrMUdUL2RyVGQyWFpuSjJaa211dnE5cm11MnQ2ejVOdUdZLzZPZzcxTkwrcEZIcFJOQWUzMlllWUFISXVDYUNvbU09PC9QPjxRPnZGY3Z4MEduRit1Rk4vQTJWU0JNR1R3VXBwbGovdHRIQ2VPTHJmTUR1ZkNTN29qakR5RThzNnhLdU1HaVNTU0ZJeDIyU0drZHZ2RFdZVmxUVThyVGRZaUZLakhSOFUwREljb2RvUTVlakJCSkx5a3ZNb09hZnhHU1NZRFZKUGNrQTlIYXdwTVNnRHF4Q0p2QzFVZ25lU1pvSHhMTGtjZkRaZ3lKaVpUYkpzOD08L1E+PERQPk42ckpzT3YweWRoTXVWdHI1blY4QjBiMk9rRHJPNVZiQVVwa0RZRm5acDRNMGZyM3YxYjF6Y3BjN2JBaXZ6WnA0ZzhXNmlubHBoSzJaM2F4OTBoeFloejdUcExPTVRtRFVTVWdFMkVNdUp1d3V1a3lMQi91bmlnU3d4OTZrZzZ0eW1QQnY1aVZuZjFGdjA5VGxiUUQ0T1BFblFlZ0FhdFBlVmE3c3NUYmtQVT08L0RQPjxEUT5nR1lVRFl5bHBMb05CVGkvWWNOMS9kSW01ZmsyNGIyT0xhQ3lUakdaZmI4VC9JalgreTJXbTRzL0didndybHEyWlYxUk13WnVvQWpDcm5WZVNJYkRLS0tjM2tvK3JYbnFRN1B1QlNtdHJXRkE4MlRwWjArdkdTZmFpai9Kajd6cEVhMlVyUWZsR2dScFFzd2x4SSsvVVdtc25GcU02K0s3Ukt3UmEvbE9lcU09PC9EUT48SW52ZXJzZVE+aEo0bHpBaFNtMVJNalRPUnJWcVJ6M2FvZDFnSHlsTWRQSTJ2MS94U2RGM0ZkR1k4dDZXNkRVNHQ1SVlPUks1TWhaeUFCYW4rVUdQam1OT2tvbGhWZEF4MTFSbm5QMkxUT2o5ZkhaR0JtN1BPeHBoL1JYd3A0RGtrVWdWcnB0VzhQWlBzTjljd1JwTHZOOEdjUFFQaDBNVlo4a2xWYnRDdlFmWERITno3cW80PTwvSW52ZXJzZVE+PEQ+Qy9rdEJiV0QvM0dydGw1VTdkZGtYWE9GLys0d2NHWVJ0Sm54YkQ4dFpXTDZBQzByRm9saWx4SmQ2VEttRjM4VVNxalMyVVJwYlFmQW9iL2pCZWN2Nyt4TkNVR2Y2SFZvMDYxYWRhejJmUzh6SHcxK2tLRC9tL0IxRktTd0FLOFZQNWZLYlJCRXppSUQ4OWIvb21JN0JYeVFHdWdWRlAvSThjNHRVVWJwODdNM0d2VUt1U3psZy9kckljYThGaEVzK3FyNENkWlhEdVhyeFF2bmVTMVMwWU0xNUlndnB3TjRsR21hVjF4QVh5Qm9SZEFDd0V1ZzF1Nmkzbko4enErZlhGaHdEMGRMOHJ1NVlEMnliWWlYM0FveXdxNE1RNHlmWm4vQm5Td2xocy9JMFZ2VkFsR2MrTGFpR3VsVWhlZUk5dUczZTVZb3RFTmlDdElOZnVkUkR3PT08L0Q+PC9SU0FLZXlWYWx1ZT4=";

        [SetUp]
        public void InternalSetUp()
        {
            SetUp();
        }

        protected virtual void SetUp()
        {
        }

        [TearDown]
        public void InternalTearDown()
        {
            IDisposable disposable;

            while (_stuffToDispose.TryPop(out disposable))
            {
                Console.WriteLine($"Disposing {disposable}");
                disposable.Dispose();
            }
        }

        protected TDisposable Using<TDisposable>(TDisposable disposable) where TDisposable : IDisposable
        {
            _stuffToDispose.Push(disposable);
            return disposable;
        }
    }
}
