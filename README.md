# Trillium

[![Generic badge](https://img.shields.io/badge/IDE-VS2022-blue.svg)](https://shields.io/)
[![Generic badge](https://img.shields.io/badge/C%23-10%2E0-blue.svg)](https://shields.io/)
[![Generic badge](https://img.shields.io/badge/%2ENet%20Core-6%2E0-blue.svg)](https://shields.io/)

![license](https://img.shields.io/github/license/VerifiedXBlockchain/VerifiedX-Trillium)
![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/VerifiedXBlockchain/VerifiedX-Trillium/dotnet.yml)
![issues](https://img.shields.io/github/issues/VerifiedXBlockchain/VerifiedX-Trillium)
![Discord](https://img.shields.io/discord/917499597692211260?label=discord)

![GitHub commit activity](https://img.shields.io/github/commit-activity/m/VerifiedXBlockchain/VerifiedX-Trillium)
![GitHub last commit](https://img.shields.io/github/last-commit/VerifiedXBlockchain/VerifiedX-Trillium)

![GitHub Release Date](https://img.shields.io/github/release-date/VerifiedXBlockchain/VerifiedX-Trillium)

Trillium is the smart contract language that will be powering our smart contracts. This is a from scratch language that has roots from C#, C, and some Javascript.

# Smart Contract Capabilities

Trillium is a **general-purpose, Turing-complete smart contract programming language** designed for blockchain applications. While it was initially developed to power self-executing NFTs (SENs), its robust architecture and comprehensive feature set make it suitable for implementing any type of smart contract or decentralized application.

## Versatile Use Cases

Trillium can be used to build a wide variety of blockchain applications:

- **Decentralized Finance (DeFi)**: Token swaps, lending protocols, liquidity pools, yield farming contracts
- **Digital Assets**: Custom token standards, fungible tokens, NFTs, receipt tokens, and hybrid asset models
- **Decentralized Autonomous Organizations (DAOs)**: Governance systems, proposal mechanisms, voting protocols
- **Financial Instruments**: Multi-signature wallets, escrow services, payment channels, vesting schedules
- **Identity & Access**: Decentralized identity systems, role-based access control, credential verification
- **Supply Chain**: Product tracking, authenticity verification, logistics management
- **Gaming & Metaverse**: In-game economies, item ownership, character progression systems
- **Legal & Compliance**: Smart legal contracts, automated compliance checking, dispute resolution
- **And Much More**: The Turing-complete nature of Trillium means if you can code it, you can deploy it

## Why Trillium Excels at Smart Contracts

The language's core properties make it particularly well-suited for blockchain development:

1. **Deterministic Execution**: Every contract produces predictable, reproducible results - essential for blockchain consensus
2. **Composable Design**: Complex contracts can be built by combining simple, auditable components
3. **Static Type Safety**: Type errors are caught at compile-time, preventing costly runtime failures
4. **Explicit State Management**: All state changes are clear and traceable, making contracts easier to audit
5. **Control Flow Analysis**: Static analysis ensures all code paths are well-defined and return properly

These properties aren't just theoretical - they're built into the language implementation through the Binder, Evaluator, and ControlFlowGraph systems, ensuring that smart contracts written in Trillium are reliable, secure, and maintainable.

# Core Features (Statements, expressions, etc)

This language is turing complete and has many features. Below are most of the current implemented features:
- Numbers (ints)
- Strings ("Any")
- Booleans (true, false)
- Addition ('+')
- Subtraction ('-')
- Multiplication ('*')
- Division ('/')
- Basic Operator Tokens(&, !, ^, |, =, (, ), >, <, [, ], :, ',', &&, !=, ^=, ==, >=, <=, and more)
- If Statements
- Else Statements
- Else If Statements
- Returns
- While Loops
- For Loops
- Do While Statements
- Block Statements
- Expressions
- Variable Declaration
- LiteralExpression,
- Name Expression
- Unary Expression
- Binary Expression
- Parenthesized Expression
- Assignment Expression
- Call Expression

# Examples

Trillium is a general-purpose smart contract language capable of implementing any blockchain application logic. While it was designed with self-executing NFTs (SENs) in mind, its Turing-complete nature and robust feature set make it suitable for DeFi protocols, DAOs, token systems, voting mechanisms, escrow services, and much more.

Below are examples demonstrating various capabilities:

## Basic Functions

Simple function demonstration:
```
function HelloPerson(name : string) : string
{
    var text = "Hello " + name
    return text
}
HelloPerson("Trillium")
```
_outputs Hello Trillium_

```
12 + 12 -> press enter
```
_outputs 24_

```
function WhoAmI(age : int, name : string)
{
    if name == "Trillium" && age == 99
    {
        print("Hi " + name)
    }
    else
    {
        print("Hello, sorry but we don't know you")
    }
}
```
_outputs "Hi Trillium" if name = "Trillium" and age = 99_
_outputs "Hello, sorry but we don't know you" for everything else_

## Variable Declarations

You can define variables as such:
```
{
    var name : string = "Some Name" (or var name = "Some Name")
    var number : int  = 22 (or var number = 22)
    var truth : bool = true (or var truth = true)
}
```

## Calculator Example

Basic two number calculator where we pass in the operator as a string:
```
function calculator(firstNum : int, secondNum : int, operator : string) : string
{
    var result = 0
    if operator == "+"
    {
        result = firstNum + secondNum
        return string(result)
    }
    else if operator == "-"
    {
        result = firstNum - secondNum
        return string(result)
    }
    else if operator == "*"
    {
        result = firstNum * secondNum
        return string(result)
    }
    else if operator == "/"
    {
        result = firstNum / secondNum
        return string(result)
    }
    else 
    {
        return "No known operator was given"
    }
}

calculator(2, 2, "+") 
```
_output is 4_

## Smart Contract Examples

### DeFi: Token Transfer Contract
```
function transferTokens(from : string, to : string, amount : int) : bool
{
    // Validate inputs
    if amount <= 0
    {
        return false
    }
    
    if from == "" || to == ""
    {
        return false
    }
    
    // Get current balances
    var fromBalance = getBalance(from)
    var toBalance = getBalance(to)
    
    // Check sufficient balance
    if fromBalance < amount
    {
        return false
    }
    
    // Execute transfer
    var newFromBalance = fromBalance - amount
    var newToBalance = toBalance + amount
    
    setBalance(from, newFromBalance)
    setBalance(to, newToBalance)
    
    return true
}
```

### Governance: Voting System
```
function castVote(proposalId : int, voter : string, voteFor : bool) : bool
{
    // Verify voter hasn't already voted
    if hasVoted(proposalId, voter)
    {
        return false
    }
    
    // Get voter's token balance (voting weight)
    var votingPower = getBalance(voter)
    
    if votingPower <= 0
    {
        return false
    }
    
    // Record vote
    recordVote(proposalId, voter)
    
    // Update vote counts
    if voteFor
    {
        var currentYes = getYesVotes(proposalId)
        setYesVotes(proposalId, currentYes + votingPower)
    }
    else
    {
        var currentNo = getNoVotes(proposalId)
        setNoVotes(proposalId, currentNo + votingPower)
    }
    
    return true
}
```

### Escrow: Conditional Release
```
function releaseEscrow(escrowId : int, releaseCode : string) : bool
{
    var expectedCode = getEscrowCode(escrowId)
    var amount = getEscrowAmount(escrowId)
    var recipient = getEscrowRecipient(escrowId)
    
    // Verify release code
    if releaseCode != expectedCode
    {
        return false
    }
    
    // Verify escrow is still active
    if amount <= 0
    {
        return false
    }
    
    // Release funds to recipient
    var recipientBalance = getBalance(recipient)
    setBalance(recipient, recipientBalance + amount)
    
    // Mark escrow as completed
    setEscrowAmount(escrowId, 0)
    
    return true
}
```

### Access Control: Role-Based Permissions
```
function executeAdminAction(user : string, action : string) : bool
{
    // Check if user has admin role
    var isAdmin = hasRole(user, "admin")
    
    if !isAdmin
    {
        return false
    }
    
    // Execute admin-only action
    if action == "mint"
    {
        var totalSupply = getTotalSupply()
        setTotalSupply(totalSupply + 1000)
        return true
    }
    else if action == "pause"
    {
        setPaused(true)
        return true
    }
    
    return false
}
```

These examples demonstrate that Trillium can handle diverse smart contract requirements, from simple value transfers to complex multi-party agreements and governance systems.

![image](https://user-images.githubusercontent.com/20599614/168458348-52a33585-0600-4070-9dec-7110809fb5d2.png)
![image](https://user-images.githubusercontent.com/20599614/168458326-18e65a71-4554-4de7-ad3a-7420cc180be1.png)

Trillium is constantly growing and will evolve over time to support more functions and have more built in features to reduce code writing.

# Language Composability

Trillium is designed from the ground up to be **composable**, meaning complex programs can be built by combining simple, well-defined components. This composability is crucial for smart contract development, where reliability and modularity are paramount.

## Function Composition

Functions in Trillium can seamlessly call other functions, enabling developers to break down complex logic into reusable components:

```
function getName(): string
{
    print("What's your name?")
    let name = input()
    return name
}

function greet(): void
{
    let name = getName()
    print("Hello " + name + "!")
}
```

This example demonstrates how `greet()` composes with `getName()` to create more sophisticated behavior from simple building blocks.

## Type-Safe Interfaces

Every function in Trillium has a well-defined interface with explicit parameter types and return types. This ensures that when functions are composed, type mismatches are caught at compile-time rather than runtime:

```
function add(a : int, b : int) : int
{
    return a + b
}

function multiply(a : int, b : int) : int
{
    return a * b
}

function calculate(x : int, y : int) : int
{
    var sum = add(x, y)
    var product = multiply(sum, 2)
    return product
}
```

The Binder (semantic analyzer) ensures that all function calls are type-correct before the program executes, preventing composition errors.

## Multi-File Modular Design

Trillium supports splitting code across multiple files, which can then be compiled together into a single program. The compilation system (`Compilation.cs`) merges multiple `SyntaxTree` objects, allowing developers to:

- Organize related functions into separate files
- Create reusable libraries of common functions
- Maintain clean separation of concerns

Example project structure:
```
project/
  ├── math.trlm       (mathematical functions)
  ├── strings.trlm    (string manipulation functions)
  └── main.trlm       (entry point that uses both)
```

## Scope Isolation

Trillium's scoping system prevents naming conflicts and ensures that composition remains predictable:

```
function outer() : int
{
    var x = 10
    {
        var x = 20  // Different scope, no conflict
        print(string(x))  // Prints 20
    }
    return x  // Returns 10
}
```

The `BoundScope` system maintains scope hierarchies, ensuring that:
- Variables are properly isolated within their scopes
- Function parameters don't conflict with global variables
- Block statements create their own scope boundaries

## Composition Guarantees

The language implementation provides several guarantees that make composition reliable:

1. **Static Function Resolution**: All function calls are resolved at compile-time (via `Binder.BindCallExpression`)
2. **Parameter Validation**: Argument count and types are verified before execution
3. **Return Type Checking**: Functions that declare return types must return the correct type on all paths
4. **No Hidden Dependencies**: Functions can only access parameters, local variables, and explicitly declared global variables

These guarantees mean that when you compose functions in Trillium, you can be confident that:
- If it compiles, the composition is type-safe
- Functions behave consistently regardless of where they're called from
- There are no hidden side effects or dependencies

# Language Determinism

Trillium is designed to be **deterministic**, meaning that given the same inputs, a program will always produce the same outputs. This property is essential for smart contracts and blockchain applications, where predictability and reproducibility are critical requirements.

## Static Type System

Trillium employs a strict static type system with compile-time type resolution:

```csharp
// From TypeSymbol.cs
public static readonly TypeSymbol Bool = new TypeSymbol("bool");
public static readonly TypeSymbol Int = new TypeSymbol("int");
public static readonly TypeSymbol String = new TypeSymbol("string");
public static readonly TypeSymbol Any = new TypeSymbol("any");
public static readonly TypeSymbol Void = new TypeSymbol("void");
```

All types are resolved during the binding phase, before execution begins. This eliminates runtime type surprises and ensures that type-related behavior is entirely predictable.

## Explicit Evaluation Model

Trillium uses a tree-walking interpreter with a defined evaluation order. Every expression type has explicit evaluation semantics defined in `Evaluator.cs`:

```
// Binary expressions always evaluate left operand first, then right
var left = EvaluateExpression(b.Left);
var right = EvaluateExpression(b.Right);
```

Arithmetic operations produce deterministic results:
```
12 + 5      // Always evaluates to 17
10 * 3      // Always evaluates to 30
20 / 4      // Always evaluates to 5
```

## Control Flow Analysis

The `ControlFlowGraph` class enables static analysis of all possible execution paths through a program:

```csharp
public static bool AllPathsReturn(BoundBlockStatement body)
{
    var graph = Create(body);
    foreach (var branch in graph.End.Incoming)
    {
        var lastStatement = branch.From.Statements.LastOrDefault();
        if (lastStatement == null || lastStatement.Kind != BoundNodeKind.ReturnStatement)
            return false;
    }
    return true;
}
```

This analysis ensures that:
- All execution paths through functions with return values terminate with a return statement
- Control flow is explicit and analyzable
- No undefined behavior from missing return statements

## No Hidden State

All program state in Trillium is explicit and traceable:

1. **Variables**: All variables must be declared before use
2. **Function Parameters**: All inputs to functions are explicit parameters
3. **Return Values**: All outputs from functions are explicit return values
4. **Scoped State**: Variable lifetime is determined by lexical scope

There is no:
- Global mutable state (except explicitly declared global variables)
- Hidden implicit parameters
- Thread-local storage
- Dynamic scope resolution

## Pure Expression Evaluation

Most expressions in Trillium are pure - they always produce the same output for the same inputs:

```
function square(x : int) : int
{
    return x * x
}

square(5)  // Always returns 25
square(5)  // Always returns 25
square(5)  // Always returns 25
```

The evaluation of expressions is deterministic:
- **Literal expressions**: Always evaluate to their literal value
- **Variable expressions**: Evaluate to the current value of the variable
- **Unary expressions**: Deterministic operations (negation, logical NOT, bitwise complement)
- **Binary expressions**: Deterministic operations (arithmetic, comparison, logical, bitwise)

## Explicit Side Effects

Operations with side effects are clearly marked and limited:

- `print(message)` - Console output (explicit I/O)
- `input()` - Console input (explicit I/O)
- Variable assignment - Explicit state mutation

All other operations are side-effect free. This makes it easy to reason about program behavior and identify non-deterministic operations.

## Deterministic Operators

Every operator in Trillium has precisely defined behavior:

### Arithmetic Operators
```
+  (Addition)        : int + int → int,  string + string → string
-  (Subtraction)     : int - int → int
*  (Multiplication)  : int * int → int
/  (Division)        : int / int → int (integer division)
```

### Comparison Operators
```
== (Equals)          : T × T → bool
!= (Not Equals)      : T × T → bool
<  (Less Than)       : int × int → bool
<= (Less Or Equal)   : int × int → bool
>  (Greater Than)    : int × int → bool
>= (Greater Or Equal): int × int → bool
```

### Logical Operators
```
&& (Logical AND)     : bool × bool → bool
|| (Logical OR)      : bool × bool → bool
!  (Logical NOT)     : bool → bool
```

### Bitwise Operators
```
&  (Bitwise AND)     : int × int → int
|  (Bitwise OR)      : int × int → int
^  (Bitwise XOR)     : int × int → int
~  (Ones Complement) : int → int
```

All operators follow standard mathematical and logical rules with no special cases or undefined behavior.

## Type Conversion Determinism

Type conversions in Trillium are explicit and predictable:

```
var x = 42
var s = string(x)     // Always converts 42 to "42"
var b = bool(1)       // Always converts non-zero to true
var i = int("123")    // Always converts "123" to 123
```

The `Conversion` class defines exact conversion rules:
- **Identity conversions**: Same type to same type (no-op)
- **Implicit conversions**: Safe conversions that preserve value
- **Explicit conversions**: Conversions that may lose precision (must be requested explicitly)

## Compile-Time Validation

The `Binder` performs extensive compile-time validation to prevent runtime surprises:

1. **Undefined variables** are caught at compile-time
2. **Type mismatches** are caught at compile-time
3. **Wrong number of arguments** is caught at compile-time
4. **Undefined functions** are caught at compile-time
5. **Invalid operators for types** are caught at compile-time

This means that if a Trillium program compiles successfully, its runtime behavior is guaranteed to be deterministic (excluding explicit I/O operations).

## Example: Deterministic Smart Contract

```
function transferTokens(fromAddress : string, toAddress : string, amount : int) : bool
{
    // Validate amount is positive
    if amount <= 0
    {
        return false
    }
    
    // Validate addresses (deterministic string comparison)
    if fromAddress == "" || toAddress == ""
    {
        return false
    }
    
    // Calculate new balances (deterministic arithmetic)
    var fromBalance = getBalance(fromAddress)
    var toBalance = getBalance(toAddress)
    
    if fromBalance < amount
    {
        return false
    }
    
    var newFromBalance = fromBalance - amount
    var newToBalance = toBalance + amount
    
    // Update balances
    setBalance(fromAddress, newFromBalance)
    setBalance(toAddress, newToBalance)
    
    return true
}
```

This function will **always** produce the same result for the same inputs:
- Same addresses and amount will always perform the same validation
- Same arithmetic operations will always produce the same balances
- Same return value based on the same conditions

## Why Determinism Matters for Smart Contracts

In blockchain and smart contract systems, determinism is critical because:

1. **Consensus**: All nodes must agree on the result of executing a contract
2. **Auditability**: Contract behavior must be predictable and verifiable
3. **Reproducibility**: Historical transactions must produce the same results when re-executed
4. **Security**: Non-deterministic behavior can be exploited for attacks

Trillium's deterministic design ensures that smart contracts written in the language meet these requirements, making it suitable for blockchain applications where predictability is paramount.

# Things to do

- Emit IL
- Nullable types
- Comments (May not do this to keep source code files smaller)

# Setup

- Install Visual Studio 2022 or latest version of Visual Code
- .Net Core 6
- Download the Trillium repo from GitHub
- Open the Trillium.sln file in Visual Studio or open the src folder in VS Code

# Testing
To write and test your own contracts first follow the setup instructions above. Next you will want to run the TrilliumI project. This will open the REPL in the CLI and allow you to being writing natively in Trillium and getting results. The REPL provides full diagnostics, so you will know exactly what is wrong within your statements of code.

# Who do I talk to? ###

- Repo owner or admin
- Other community or team contact
- https://discord.gg/PnS2HRETDh

# People to Thank and Sources

The language was modeled after the language courses from Microsoft's Immo Landwerth and adapted further to work within the ReserveBlocks core wallet infastructure.

Some other references to site are (please note none of these people worked on the project, but rather provided material to help make this. They are in no way affiliated with this project):

- Clinton L. Jeffery and his Books
- Jon Skeet and his C# In Depth books
- Alex Williams and his debugging methodology
- Immo Landwerth and his series

# License

Trillium is released under the terms of the MIT license. See [COPYING](COPYING) for more
information or see https://opensource.org/licenses/MIT.
