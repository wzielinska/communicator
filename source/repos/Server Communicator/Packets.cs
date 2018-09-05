using System;

public class Packets
{
    public const int Hello = 59; //Hello
    public const byte Done = 1; //Done
    public const byte Login = 2; //Login
    public const byte Register = 3; //Register
    public const byte UsernameError = 4; //Za długa nazwa użytkownika
    public const byte PasswordError = 5; //Hasło nie pasuje do wzorca 
    public const byte Exists = 6; //Uzytkownik juz istnieje
    public const byte NoExists = 7; //Uzytkownik nie instnieje w bazie
    public const byte WrongPassword = 8; //Bledne haslo
    public const byte IsAvailable = 9; //Zapytanie o dostepnosc uzytkownika
    public const byte Available = 10; //Dostepny lub nie
    public const byte SendMessage = 11;//Wyslanie wiadomosci
    public const byte Received = 12; //Otrzymana wiadomosc
}
