namespace umfgcloud.programcaoiii.vendas.api.DTO
{
    public class VendedorDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; }= string.Empty;

        public VendedorDto() 
        { 

        }

        public VendedorDto(string nome, string email, string telefone)
        {
            Nome = nome;
            Email = email;
            Telefone = telefone?? "";
        }
    }
}
