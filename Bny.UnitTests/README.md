# Bny.UnitTests
This is the code for the library.

## In this directory
- **Tester.cs** - the main api (contains for example `Tester.TestAll()`)
- **Asserter.cs** - the asserter class (contains for example `Asserter.Assert(/* assertion */)`)
- **Assertion.cs** - structure containing info about a specific assertion
- **LogAmount.cs**, **TestFunction.cs**, **UnitTestAttribute.cs** - definition of the types `LogAmount` enum, `TestFunction` delegate and `UnitTest` attribute respectively
- **Color.cs** - internal definitions of ANSI escape codes used by the library
- **Doxyfile** - configuration for generating doxygen documentation
- **Global.cs** - things that aren't tied to specific type (currently only some doxygen comments)
