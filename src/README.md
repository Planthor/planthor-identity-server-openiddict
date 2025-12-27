# Testing in .NET

## Test types

Automated tests are a great way to ensure that the application code does what its authors intend. This article covers unit tests, integration tests, and load tests.

### Unit tests

A unit test is a test that exercises individual software components or methods, also known as a "unit of work." Unit tests should only test code within the developer's control. They don't test infrastructure concerns. Infrastructure concerns include interacting with databases, file systems, and network resources.

### Integration tests

An integration test differs from a unit test in that it exercises two or more software components' ability to function together, also known as their "integration." These tests operate on a broader spectrum of the system under test, whereas unit tests focus on individual components. Often, integration tests do include infrastructure concerns.

### Load tests

A load test aims to determine whether or not a system can handle a specified load. For example, the number of concurrent users using an application and the app's ability to handle interactions responsively.
