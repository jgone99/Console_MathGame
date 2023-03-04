
string mathGameTitle = "Console Math Game";

string GHViewTitle = "Game History View";

string prompt =
    "1. add\n" +
    "2. subtract\n" +
    "3. multiply\n" +
    "4. divide\n" +
    "5: view game history\n\n" +
    "cmd: ";

string GHMenu =
    "1. previous\n" +
    "2. next\n" +
    "3. exit\n\n" +
    "cmd: ";

List<string> messages = new List<string>();

LinkedList<MathGame> gameHistory = new LinkedList<MathGame>();

void clearWindow()
{
    (int cX, int cY) = Console.GetCursorPosition();
    string newWindow = "";
    for (int i = 0; i < Console.WindowHeight + cY; i++)
    {
        newWindow += "\n";
    }
    Console.Write(newWindow);
    Console.SetCursorPosition(cX, cY);
}

bool isValidOption(string option, int optionCount)
{
    if (string.IsNullOrEmpty(option))
        return false;

    option = option.Trim();

    if ((option.Length == 1) && char.IsDigit(option[0]))
    {
        return char.GetNumericValue(option[0]) <= optionCount && option[0] >= '1';
    }

    return false;
}

bool isValidArg(string arg)
{
    if (string.IsNullOrEmpty(arg))
        return false;

    arg = arg.Trim();

    for (int i = 0; i < arg.Length; i++)
    {
        if (!char.IsDigit(arg[i]) && (arg[i] != '-' || i != 0))
            return false;
    }

    return true;
}

void displayMessages()
{
    messages.ForEach(str => Console.Write($"\n{str}\n"));
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

    Console.Write($"\n{GHViewTitle}\n");

    while (inGHView)
    {
        clearWindow();
        displayMessages();
        Console.Write($"\ngame {gameIndex}/{gameHistory.Count}\n\n");
        gameNode.Value.print();
        Console.Write($"\n\n{GHMenu}");
        cmdString = Console.ReadLine();

        if (!isValidOption(cmdString, 3))
        {
            messages.Add("invalid option");
            continue;
        }

        cmd = int.Parse(cmdString.Trim());

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
int total = 0, arg, cmd;

do
{
    clearWindow();

    displayMessages();

    Console.Write($"\n{mathGameTitle}\n");

    Console.Write($"\ntotal : {total}\n\n{prompt}");
    cmdString = Console.ReadLine();

    if (!isValidOption(cmdString, 5))
    {
        messages.Add("invalid option");
        continue;
    }

    cmd = int.Parse(cmdString.Trim());

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

    if (!isValidArg(argString))
    {
        messages.Add("invalid argument");
        continue;
    }

    arg = int.Parse(argString.Trim());

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
            total /= arg;
            break;
    }

    gameHistory.AddFirst(new MathGame(total, arg, cmd));
}
while (true);

struct MathGame
{
    int result;
    int arg;
    int operation;
    public MathGame(int result, int arg, int operation)
    {
        this.result = result;
        this.arg = arg;
        this.operation = operation;
    }
    public void print()
    {
        Console.Write($"total: {result}\narg: {arg}\noperation: {operation}");
    }
}