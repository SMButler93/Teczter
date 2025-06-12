# Welcome to Teczter!

This is a manual regression testing tool. Users can create tests, execute tests, assign tests to others and monitor test states. The aim of this project is to create a free and simple testing tool, to replace the use of pricey alternatives.

## Overview:

Here are the basic models that need to be understood in order to understand the overall structure and functionality of the application:

### Test:
A test is a set of test steps which specify a set of instructions in a specific order. Each test has a title, a description and a department by whom the test is owned by/relates to. A test can have URL's assigned. These could be used to provide the starting point of the test etc. Tests are not actually executed. Instead we have executions (see below) which take that test and allow it to be executed. This allows for tests to be reused and updated in a single place reducing the amount of data stored. Only users with admin level permissions can make alterations to a test.

### Test step:
A test step is a single step/instruction that is part of test. A test step can be individualy marked as passed or failed. Each test step has a step placement (this dictates the order in which the steps should be completed in order to complete the entire test) and instructions. A test step can have URL's assigned. These may be used for pointing to certain pages, resources etc that are relevant to that step. Only users with admin level permissions can make alterations to a test step.

### Execution:
An Execution is what is executed by a user in order to determine if a test is passing or not. An execution will have an associated test and it will present the test details and test steps so that the user can complete them, and the execution will record and persist the results.

### Execution group:
An Execution group is a group of executions. The idea behind a group is to allow users to tie an execution group to a specific product or a specific version of a product. This will allow test results to be tracked and monitored over multiple release versions. Restrictions are put in place so that when an execution group is closed, no further changes to any of the containing executions can be made. Only users with admin level permissions can make alterations to an execution group.

### User:
A user is someone using the system. A user can have executions assigned to them and can then execute them. There are various user permissions which restricts some users from making changes to certain data. This includes the ability to change test details and titles, as well as assign executions to other users and more.

## Technology details:
This is currently running on .NET 9. The aim, is to update the project to always use the latest stable version of .NET.

The database is managed via an SSDT project. Initially, the project was built using a code first approach, where database schema was dictated and controlled by the configuration classes for each entity. Recently (mid May 2025) the project was switched over to use SSDT. The migration files where removed and the configuration classes where updated to reflect this etc. Entity framework is the ORM used within this project.

Angular is used for the frontend. PrimeNg is used for various components within the frontend.

## Planned work:
Moving forward with this project, there is still currently a lot to do. Below is a list of upcoming tasks. This is by no means an exhaustive list. This list will change as and when new work is identified and tasks are completed.

* Implement authentication and authroisation
* Build MVP UI
* Implement notification emails
* Use domain events and CQRS where appropriate to reduce coupling and improve scalability
