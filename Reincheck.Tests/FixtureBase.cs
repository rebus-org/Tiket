using System;
using System.Collections.Concurrent;
using NUnit.Framework;

namespace Reincheck.Tests
{
    public abstract class FixtureBase
    {
        readonly ConcurrentStack<IDisposable> _stuffToDispose = new ConcurrentStack<IDisposable>();

        protected const string ValidKey =
            @"<RSAKeyValue><Modulus>qFUWOS28maQ8EaNJ6CeS9wuB0iv8CLTDJj0JuY7x8mrYtKMdxbPCwvlOP5/HRSZyGyeHkvIHPpjbwXeQY4LGgZH5KLLrVsGMULAeFM5CG+J2GBzvsyG3XW7shkSv0MnmbvcSOg6GdGsYB37JC6Nya/COjMUr7tSSxNdE0d/SVSOk2XtIl5oVCNhJip+JSDkhZdD6AFxE16YgTB7N5bnhDSHM8oaN+qFh6fNUjrxFS4/bV33APbv1vxblB7+Ve2kD3lTnwb75ZBAszvASH+ofxOlvOmobsXVwiJCKLnRWGsFarPQoCpQ2w0CQtNuAzynhhrlvslwiVHIEY4UIq2MG8w==</Modulus><Exponent>AQAB</Exponent><P>0QDz1fSji71SvrM5TiLp5oXdiB9GvGQ9nF/xZ4jumAxP5ApF8YgKF7CZfHsnBMNl9kChMfPgcMyVy02FP+fOgV6k4N7+TktZWd+NWQaZIQEj3chQpM14KsSWDR0bDJxGq7WZ/MKJuHAnZH57NY2qdwcZkTjCj/oAhTEr9XwxmZU=</P><Q>zi7v4IaudreSMOv1NgSlxNbGra+Ynt6NQyP/dVHKfLQkrbPdOs4mS5XNUA4vSU7GXG4Gxb884oZMU5NMTvwAWjVMOIy+LRcvj3LbFTa1NXWg8uoM69ySWMaS0behg/hQC0PTEnJ4FSTg8KvQ1uLUryRKN/IvWfYYHtMRvjj+TGc=</Q><DP>MJK3GI9tful2FInUlJs6nu+Xh641I01soC4QDY7eXInn+0iD7tk99zl8wlhgvhOnW66zh8d07uItIf2XLT3zWU+ko/pPQeTzFBcHY5xL9EaujjmivFpgRhhAAzYzdh/PsNJYwkx381xlW06l1dFhv53vzDdTyGBCxLGj1L6TTG0=</DP><DQ>i4F4rsuSCiOFF/LfdFBKtVe5EFXqhzwACri1pXTU8/GTi7BSdPJ9ONFAHPWsCwbw8iNEE9KkaIUD6dyVWi/qMR801mJsXuf8Nyw4ji75BxXy88lnOsOAhe698oY0E64Uwrp+e1HWbEAKru0iHfNWgrakPRxx64W2pkycCcBYV+s=</DQ><InverseQ>UiV2Sd9EoR53Y3/PrZhZW0FqQfek9AqRvXwOmDD+r6PUbpU+wJZ8AN832DBoK5REq5xLatrLmM40UgQ+41pi/3YnSMsYM07Z6tmLA31wr4S4DnQGjc7+lGI3DsKD1Zz7ao+rVrpkVBen0dGxg7eqYDYVEZZbWnv/8H5hrghUSrg=</InverseQ><D>CIdbG2WKVzx/gKNF1Nngc9zk5HmvCN/jvVW9XuXzs0UfmrNjCEfN1+Qax1V4mrPSdza/Az55CH5lL/23VptjJGB5f8rk6p3e1p29wG8c4V3NfvZEXM6aBEVg/YKQZ+M65idliISA4ngknQia9hh/bSGgj1hUHAMwNoHzrjCJbx/MqqWP1Rf/vPhI94lf6aI8ZUWb4SgDi7O6KgpN2rFdKItkYwigA11oQXAW2jMq3gEBhRBj20YjU5ku6v0HX3oDjZVV+Hyi1r/8NtyGfK5I+kkXsSv1hVgxDrvIbkDF5MDXT3rKUtPauWlG9wPCsSM0X/HMgLLsjYULwXgTQXAgvQ==</D></RSAKeyValue>";

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
