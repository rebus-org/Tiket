﻿# Changelog

## 1.0.1
* Made Tiket

## 1.0.2
* Fixed ilmerge

## 1.0.3
* Changed to 256 bit AES

## 1.0.4
* Provide `ThrowIfInvalid` method on the decoding result

## 2.0.0
* Add graceful handling of garbage input
* Target .NET Standard 2.0

## 3.0.0
* Don't ZIP signature, because that makes it unstable across frameworks. This is a BREAKING change, unfortunately, so all generated tokens will become invalid. Sorry about that. 😿