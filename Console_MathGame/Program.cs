
using System.Text;

const string mathGameTitle = "Console Math Game";
const string GHViewTitle = "Game History View";
const string prompt =
    "1. add\n" +
    "2. subtract\n" +
    "3. multiply\n" +
    "4. integer divide\n" +
    "5: view game history\n\n" +
    "cmd: ";
const string GHMenu =
    "1. previous\n" +
    "2. next\n" +
    "3. exit\n\n" +
    "cmd: ";

List<string> messages = new List<string>();

LinkedList<MathGame> gameHistory = new LinkedList<MathGame>();

void clearWindow()
{
    (int cX, int cY) = Console.GetCursorPosition();
    StringBuilder newWindow = new StringBuilder();
    newWindow.Append('\n', Console.WindowHeight + cY);
    Console.Write(newWindow);
    Console.SetCursorPosition(cX, cY);
}

bool isValidOption(string option, int optionCount, out int value)
{
    if (int.TryParse(option, out value))
    {
        return value >= 1 && value <= optionCount;
    }
    return false;
}

void divide(ref int result, int arg)
{
    if (arg == 0)
    {
        messages.Add("Cannot divide by zero");
        return;
    }
    result /= arg;
}

void displayMessages()
{
    messages.ForEach(str => Console.Write($"\n--- ALERT: {str} ---\n"));
    messages.Clear();
}

bool GHNext(ref LinkedListNode<MathGame> node)
{
    if (node.Next == null)
    {
        messages.Add("End of game history reached");
        return false;
    }
    node = node.Next;
    return true;
}

bool GHPrev(ref LinkedListNode<MathGame> node)
{
    if (node.Previous == null)
    {
        messages.Add("Front of game history reached");
        return false;
    }
    node = node.Previous;
    return true;
}

void gameHistoryView()
{
    if (gameHistory.Count == 0)
    {
        messages.Add("Game history is empty");
        return;
    }

    bool inGHView = true;
    string? cmdString;
    int cmd;
    LinkedListNode<MathGame>? gameNode = gameHistory.First;
    int gameIndex = gameHistory.Count;

    while (inGHView)
    {
        clearWindow();

        Console.Write($"\n{GHViewTitle}\n");

        displayMessages();

        Console.Write($"\ngame {gameIndex}/{gameHistory.Count}\n\n");
        gameNode.Value.print();
        Console.Write($"\n\n{GHMenu}");
        cmdString = Console.ReadLine();

        if (!isValidOption(cmdString, GHMenu.Count(c => c == '\n') - 1, out cmd))
        {
            messages.Add("invalid option");
            continue;
        }

        switch (cmd)
        {
            case 1:
                if (GHNext(ref gameNode)) gameIndex--;
                break;
            case 2:
                if (GHPrev(ref gameNode)) gameIndex++;
                break;
            case 3:
                inGHView = false;
                break;
        }
    }
}

string? cmdString, argString;
int total = 0, arg, cmd, prev;

do
{
    clearWindow();

    Console.Write($"\n{mathGameTitle}\n");
    displayMessages();

    Console.Write($"\ntotal : {total}\n\n{prompt}");
    cmdString = Console.ReadLine();

    if (!isValidOption(cmdString, prompt.Count(c => c == '\n') - 1, out cmd))
    {
        messages.Add("invalid option");
        continue;
    }

    if (cmd == 4 && (total < 0 || total > 100))
    {
        messages.Add("Dividend must be between 0 and 100");
        continue;
    }
    else if (cmd == 5)
    {
        gameHistoryView();
        continue;
    }

    Console.Write("enter arg: ");

    argString = Console.ReadLine();

    if (!int.TryParse(argString, out arg))
    {
        messages.Add("invalid argument");
        continue;
    }

    prev = total;

    switch (cmd)
    {
        case 1:
            total += arg;
            break;
        case 2:
            total -= arg;
            break;
        case 3:
            total *= arg;
            break;
        case 4:
            divide(ref total, arg);
            break;
    }

    gameHistory.AddFirst(new MathGame(prev, arg, total, cmd));
}
while (true);

struct MathGame
{
    int[] operands = new int[2];
    int result;
    int operation;
    public MathGame(int arg1, int arg2, int result, int operation)
    {
        this.operands[0] = arg1;
        this.operands[1] = arg2;
        this.result = result;
        this.operation = operation;
    }
    public void print()
    {
        Console.Write($"{operands[0]} {(operation == 1 ? '+' : operation == 2 ? '-' : operation == 3 ? '*' : '/')} {operands[1]} = {result}");
    }
}