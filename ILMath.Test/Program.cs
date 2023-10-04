using ILMath;

var expression = Console.ReadLine();
while (expression is null)
    expression = Console.ReadLine();

// Create a new evaluation context
var context = EvaluationContext.CreateDefault();

// Register a custom function
context.RegisterFunction("myFunction", parameters => parameters[0] + parameters[1]);

// Register a custom variable
context.RegisterVariable("myVariable", 5);

// Create a new evaluator
var evaluator = MathEvaluation.CompileExpression(expression);

// Evaluate the expression
var result = evaluator.Invoke(context);

// Print the result
Console.WriteLine(result);