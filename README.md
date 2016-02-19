# Tiket

A simple .NET token library.

Inspired by [JWT] - not quite JWT though, because encryption is mandatory, and only 256 bit AES is available.

The principle is the same though: A secure token consists of a bunch of tamper-proof claims.

## How to use it?

First, you get yourself a secret key to use for asymmetric encryption like this:

    Console.WriteLine(KeyMan.GenerateKey());

which will generate a new key that you can copy/paste and use. With your `secretKey` in hand, you
can get yourself a key manager like this:

	var k = new KeyMan(secretKey);

which you can then use to encode tokens like this:

	var properties = new Dictionary<string, string> {{"username", "muggie"}};

	var token = k.Encode(properties);

where `token` will now be a string that contains the information given in the `properties` above.
The simple username property above is in principle enough to provide a means of authenticating
a user, since the token cannot be tampered with.

When the time comes to use the information from the token, you can pass it to the key manager
to have it decoded like this:

     var result = k.Decode(token);

where `result` will now contain the result of decoding the token. Now you must check to see if
the token was valid, which you may do by checking the `IsValid` property of it - to make things
simple, you will probably want to do something like this:

	if (!result.IsValid)
	{
		throw new SecurityException("uh oh!!!");
	}

And that's it :)

[JWT]: https://jwt.io/