namespace umfgcloud.programcaoiii.vendas.api.Entidades
{
    public class Vendedor : AbstractEntidade
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }

        protected Vendedor()
        {

        }

        public Vendedor(string nome, string email, string telefone)
        {
            Nome = nome;
            Email = email;
            Telefone = telefone;
        }
    }
}
