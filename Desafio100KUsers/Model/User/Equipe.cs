namespace Desafio100KUsers.Model.User;

public class Equipe
{
    public string Nome { get; set; } = string.Empty;
    public bool Lider { get; set; }
    public Projeto[] Projetos { get; set; } = [];
}
