namespace Desafio100KUsers.Model.User;

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Nome { get; set; } = string.Empty;
    public int Idade { get; set; }
    public int Score { get; set; }
    public bool Ativo { get; set; }
    public string Pais { get; set; } = string.Empty;
    public Equipe Equipe { get; set; } = new();
    public Log[] Logs { get; set; } = [];
}