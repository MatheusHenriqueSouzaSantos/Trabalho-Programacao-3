using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using umfgcloud.programcaoiii.vendas.api.Contexto;
using umfgcloud.programcaoiii.vendas.api.DTO;
using umfgcloud.programcaoiii.vendas.api.Entidades;

namespace umfgcloud.programcaoiii.vendas.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string connection = "Server=localhost;" +
                "Port=3307;" +
                "Database=umfg_vendas;" +
                "Uid=root;Pwd=root";

            var builder = WebApplication.CreateBuilder(args);

            // configuração de acesso ao banco de dados
            builder.Services.AddDbContext<ContextoVenda>(option =>
                option.UseMySQL(connection));

            var app = builder.Build();

            //mapeamento dos end-points

            app.MapGet("/clientes", (ContextoVenda contexto) => 
            { 
                return contexto.Clientes.Where(x=>x.IsAtivo).ToList();
            });
            app.MapGet("/clientes/{id}", (string id, ContextoVenda contexto) =>
            {
                try
                {
                    Guid idConvertido;
                    if (!Guid.TryParse(id, out idConvertido))
                    {
                        return Results.BadRequest("id no formato inválido de GUID");
                    }
                    Cliente? clienteVindoDoBanco = contexto.Clientes.FirstOrDefault(x => x.Id == idConvertido && x.IsAtivo);
                    if (clienteVindoDoBanco == null)
                    {
                        return Results.NotFound("Cliente não Encontrado!!");
                    }
                    return Results.Ok(clienteVindoDoBanco);

                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });
            app.MapPost("/clientes", (ClienteDto dto,ContextoVenda contexto) =>
            {
                try
                {
                    Cliente clienteASalvar = new Cliente(dto.Nome,dto.Cpf,dto.Endereco,dto.Telefone);
                    contexto.Clientes.Add(clienteASalvar);
                    contexto.SaveChanges();
                    return Results.Ok(clienteASalvar);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });
            app.MapPut("/clientes/{id}", (string id,ClienteDto dto, ContextoVenda contexto) =>
            {
                try
                {
                    Guid idConvertido;
                    if (!Guid.TryParse(id, out idConvertido))
                    {
                        return Results.BadRequest("id no formato inválido de GUID");
                    }
                    Cliente? clienteVindoDoBanco = contexto.Clientes.FirstOrDefault(x => x.Id == idConvertido && x.IsAtivo);
                    if (clienteVindoDoBanco == null)
                    {
                        return Results.NotFound("Cliente não Encontrado!!");
                    }
                    clienteVindoDoBanco.AtualizarDataAtualizacao();
                    clienteVindoDoBanco.Nome = dto.Nome;
                    clienteVindoDoBanco.CPF = dto.Cpf;
                    clienteVindoDoBanco.Endereco = dto.Endereco;
                    clienteVindoDoBanco.Telefone = dto.Telefone;
                    contexto.Clientes.Update(clienteVindoDoBanco);
                    contexto.SaveChanges();

                    return Results.Ok(clienteVindoDoBanco);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });
            app.MapDelete("/clientes/{id}", (string id, ContextoVenda contexto) =>
            {
                try
                {
                    Guid idConvertido;
                    if (!Guid.TryParse(id, out idConvertido))
                    {
                        return Results.BadRequest("id no formato inválido de GUID");
                    }
                    Cliente? clienteVindoDoBanco = contexto.Clientes.FirstOrDefault(x => x.Id == idConvertido && x.IsAtivo);
                    if (clienteVindoDoBanco == null)
                    {
                        return Results.NotFound("Cliente não Encontrado!!");
                    }
                    clienteVindoDoBanco.Inativar();
                    contexto.Clientes.Update(clienteVindoDoBanco);
                    contexto.SaveChanges();

                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            app.MapGet("/produtos", (ContextoVenda contexto) =>
            {
                return contexto.Produtos.ToList();
            });

            app.MapGet("/produtos/{id}", (string id, ContextoVenda contexto) =>
            {
                try
                {
                    Guid idConvertido;
                    if (!Guid.TryParse(id, out idConvertido))
                    {
                        return Results.BadRequest("id no formato inválido de GUID");
                    }
                    Produto? produtoVindoDoBanco = contexto.Produtos.FirstOrDefault(x => x.Id == idConvertido && x.IsAtivo);
                    if (produtoVindoDoBanco == null)
                    {
                        return Results.NotFound("produto não Encontrado!!");
                    }
                    return Results.Ok(produtoVindoDoBanco);

                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });
            app.MapPost("/produtos", (ProdutoDto dto, ContextoVenda contexto) =>
            {
                try
                {
                    if (dto.PrecoCompra < 0)
                    {
                        return Results.BadRequest("O preço de Compra não pode ser negativo!!!");
                    }
                    if (dto.PrecoVenda < 0)
                    {
                        return Results.BadRequest("O preço de venda não pode ser negativo!!!");
                    }
                    if (dto.Estoque < 0)
                    {
                        return Results.BadRequest("A quantidade em estoque não pode ser negativo!!!");
                    }
                    Produto produtoASalvar = new Produto(dto.EAN, dto.Descricao, dto.PrecoCompra, dto.PrecoVenda,dto.Estoque);
                    contexto.Produtos.Add(produtoASalvar);
                    contexto.SaveChanges();
                    return Results.Ok(produtoASalvar);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });
            app.MapPut("/produtos/{id}", (string id, ProdutoDto dto, ContextoVenda contexto) =>
            {
                try
                {
                    Guid idConvertido;
                    if (!Guid.TryParse(id, out idConvertido))
                    {
                        return Results.BadRequest("id no formato inválido de GUID");
                    }
                    if (dto.PrecoCompra < 0)
                    {
                        return Results.BadRequest("O preço de Compra não pode ser negativo!!!");
                    }
                    if (dto.PrecoVenda < 0)
                    {
                        return Results.BadRequest("O preço de venda não pode ser negativo!!!");
                    }
                    if (dto.Estoque < 0)
                    {
                        return Results.BadRequest("A quantidade em estoque não pode ser negativo!!!");
                    }
                    Produto? produtoVindoDoBanco = contexto.Produtos.FirstOrDefault(x => x.Id == idConvertido && x.IsAtivo);
                    if (produtoVindoDoBanco == null)
                    {
                        return Results.NotFound("produto não Encontrado!!");
                    }
                    produtoVindoDoBanco.AtualizarDataAtualizacao();
                    produtoVindoDoBanco.EAN = dto.EAN;
                    produtoVindoDoBanco.Descricao = dto.Descricao;
                    produtoVindoDoBanco.PrecoCompra= dto.PrecoCompra;
                    produtoVindoDoBanco.PrecoVenda = dto.PrecoVenda;
                    produtoVindoDoBanco.Estoque = dto.Estoque;
                    contexto.Produtos.Update(produtoVindoDoBanco);
                    contexto.SaveChanges();

                    return Results.Ok(produtoVindoDoBanco);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });
            app.MapDelete("/produtos/{id}", (string id, ContextoVenda contexto) =>
            {
                try
                {
                    Guid idConvertido;
                    if (!Guid.TryParse(id, out idConvertido))
                    {
                        return Results.BadRequest("id no formato inválido de GUID");
                    }
                    Produto? produtoVindoDoBanco = contexto.Produtos.FirstOrDefault(x => x.Id == idConvertido && x.IsAtivo);
                    if (produtoVindoDoBanco == null)
                    {
                        return Results.NotFound("Produto não Encontrado!!");
                    }
                    produtoVindoDoBanco.Inativar();
                    contexto.Produtos.Update(produtoVindoDoBanco);
                    contexto.SaveChanges();

                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });
            app.MapGet("/vendas", (ContextoVenda contexto) =>
            {
                return  Results.Ok(contexto.Vendas.Include(x=>x.Cliente).Include(x=>x.Vendedor).Where(x=>x.IsAtivo).ToList());
            });

            app.MapGet("/vendas/{id}", (string id, ContextoVenda contexto) =>
            {
                try
                {
                    Guid idConvertido;
                    if (!Guid.TryParse(id, out idConvertido))
                    {
                        return Results.BadRequest("id no formato inválido de GUID");
                    }
                    Venda? vendaVindoDoBanco = contexto.Vendas.Include(x=>x.Cliente).Include(x=>x.Vendedor).FirstOrDefault(x => x.Id == idConvertido && x.IsAtivo);
                    if (vendaVindoDoBanco == null)
                    {
                        return Results.NotFound("Venda não Encontrado!!");
                    }
                    return Results.Ok(vendaVindoDoBanco);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            app.MapPost("/vendas", ([FromBody] TransacaoDTO.TransacaoCapaRequest dto,
                ContextoVenda contexto) =>
            {
                try
                {
                    Cliente? cliente = contexto
                        .Clientes
                        .FirstOrDefault(x => x.Id == dto.IdCliente && x.IsAtivo);

                    if (cliente == null)
                        return Results.NotFound("Cliente não cadastrado!");

                    Vendedor? vendedor = contexto
                       .Vendedores
                       .FirstOrDefault(x => x.Id == dto.IdVendedor && x.IsAtivo);

                    if (vendedor == null)
                        return Results.NotFound("Vendedor não cadastrado!");

                    Venda vendaCriada = new Venda(dto.IdCliente,cliente,dto.IdVendedor,vendedor);

                    contexto.Vendas.Add(vendaCriada);
                    contexto.SaveChanges();

                    return Results.Created($"vendas/{vendaCriada.Id}", vendaCriada);
                }
                catch (Exception ex) 
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            app.MapPut("/vendas/{id}", (string id, [FromBody] TransacaoDTO.TransacaoCapaRequest dto,
                ContextoVenda contexto) =>
            {
                try
                {
                    Cliente? cliente = contexto
                        .Clientes
                        .FirstOrDefault(x => x.Id == dto.IdCliente && x.IsAtivo);

                    if (cliente == null)
                        return Results.NotFound("Cliente não cadastrado!");

                    Vendedor? vendedor = contexto
                       .Vendedores
                       .FirstOrDefault(x => x.Id == dto.IdVendedor && x.IsAtivo);

                    if (vendedor == null)
                        return Results.NotFound("Vendedor não cadastrado!");

                    Guid idConvertidoVenda;
                    if (!Guid.TryParse(id, out idConvertidoVenda))
                    {
                        return Results.BadRequest("id no formato inválido de GUID");
                    }
                    Venda? vendaVindaDoBanco = contexto.Vendas.Include(x=>x.Cliente).Include(x=>x.Vendedor).FirstOrDefault(x => x.Id == idConvertidoVenda && x.IsAtivo);
                    if (vendaVindaDoBanco == null)
                    {
                        return Results.NotFound("Venda não Encontrado!!");
                    }
                    vendaVindaDoBanco.AtualizarDataAtualizacao();
                    vendaVindaDoBanco.ClienteId = dto.IdCliente;
                    vendaVindaDoBanco.Cliente= cliente;
                    vendaVindaDoBanco.VendedorId = dto.IdVendedor;
                    vendaVindaDoBanco.Vendedor = vendedor;
                    contexto.Vendas.Update(vendaVindaDoBanco);
                    contexto.SaveChanges();

                    return Results.Ok(vendaVindaDoBanco);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            app.MapDelete("/vendas/{id}", (string id, ContextoVenda contexto) =>
            {
                try
                {
                    Guid idConvertido;
                    if (!Guid.TryParse(id, out idConvertido))
                    {
                        return Results.BadRequest("id no formato inválido de GUID");
                    }
                    Venda? vendaVindoDoBanco = contexto.Vendas.FirstOrDefault(x => x.Id == idConvertido && x.IsAtivo);
                    if (vendaVindoDoBanco == null)
                    {
                        return Results.NotFound("Venda não Encontrado!!");
                    }
                    vendaVindoDoBanco.Inativar();
                    contexto.Vendas.Update(vendaVindoDoBanco);
                    contexto.SaveChanges();

                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            app.MapPost("/vendas/{idVenda}/itens", (
                string idVenda,
                TransacaoDTO.TransacaoItemRequest dto,
                ContextoVenda contexto) =>
            {
                try
                {
                    Guid idVendaConvertido = Guid.Parse(idVenda);

                    Venda? venda = contexto
                        .Vendas
                            .Include(x => x.Cliente)
                            .Include(x => x.Itens)
                                .ThenInclude(x => x.Produto)
                            .FirstOrDefault(x => x.Id == idVendaConvertido && x.IsAtivo);

                    if (venda == null)
                        return Results.NotFound("Venda não Encontrada!");

                    Produto? produto = contexto
                        .Produtos
                        .FirstOrDefault(x => x.Id == dto.IdProduto && x.IsAtivo);

                    if (produto == null)
                        return Results.NotFound("Produto não Encontrado!");

                    if (dto.Quantidade <= 0.0m)
                        return Results.BadRequest("A quantidade do Produto não pode ser negativa!");

                    if (produto.Estoque - dto.Quantidade < 0.0m)
                        return Results.BadRequest("Não há estoque suficiente para venda!");

                    produto.AbaterEstoque(dto.Quantidade);

                    ItemVenda itemVenda = new ItemVenda(produto, dto.Quantidade);

                    venda.AdicionarItem(itemVenda);

                    contexto.ItensVenda.Add(itemVenda);
                    contexto.Produtos.Update(produto);
                    contexto.Vendas.Update(venda);

                    contexto.SaveChanges();

                    return Results.Created($"vendas/{idVenda}", venda);
                }
                catch (Exception ex) 
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            app.MapDelete("/vendas/{idVenda}/itens/{idItem}", (string idVenda,
                string idItem, ContextoVenda contexto) =>
            {
                try
                {
                    Guid idVendaConvertido = Guid.Parse(idVenda);
                    Guid idItemVendaConvertido = Guid.Parse(idItem);

                    Venda? venda = contexto
                        .Vendas
                        .Include(x => x.Cliente)
                        .Include(x => x.Itens)
                            .ThenInclude(x => x.Produto)
                        .FirstOrDefault(x => x.Id == idVendaConvertido && x.IsAtivo);

                    if (venda == null)
                        return Results.NotFound("Venda não Encontrada!");

                    ItemVenda? itemVenda = venda
                        .Itens
                        .FirstOrDefault(x => x.Id == idItemVendaConvertido && x.IsAtivo);

                    if (itemVenda == null)
                        return Results.NotFound("Item de venda não cadastrado");

                    itemVenda.Produto.AdicionarEstoque(itemVenda.Quantidade);
                    venda.RemoverItem(itemVenda);

                    contexto.Produtos.Update(itemVenda.Produto);
                    contexto.ItensVenda.Remove(itemVenda);
                    contexto.Vendas.Update(venda);

                    contexto.SaveChanges();

                    return Results.Ok(venda);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            app.MapGet("/vendedores", (ContextoVenda contexto) =>
            {
                return contexto.Vendedores.Where(x=>x.IsAtivo).ToList();
            });

            app.MapGet("/vendedores/{id}", (string id, ContextoVenda contexto) =>
            {
                try
                {
                    Guid idConvertido;
                    if (!Guid.TryParse(id, out idConvertido))
                    {
                        return Results.BadRequest("id no formato inválido de GUID");
                    }
                    Vendedor? vendedorVindoDoBanco = contexto.Vendedores.FirstOrDefault(x => x.Id == idConvertido && x.IsAtivo);
                    if (vendedorVindoDoBanco == null)
                    {
                        return Results.NotFound("Vendedor não Encontrado!!");
                    }
                    return Results.Ok(vendedorVindoDoBanco);

                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });
            app.MapPost("/vendedores", (VendedorDto dto, ContextoVenda contexto) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(dto.Nome))
                    {
                        return Results.BadRequest("O nome não deve ser vazio");
                    }
                    if (dto.Nome.Length < 3)
                    {
                        return Results.BadRequest("O nome do vendedor deve ter no mínimo 3 caracteres!!");
                    }
                    if (string.IsNullOrWhiteSpace(dto.Email))
                    {
                        return Results.BadRequest("O Email não deve ser vazio");
                    }
                    if(!Regex.IsMatch(dto.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    {
                        return Results.BadRequest("O email deve vir em um formato válido");
                    }
                    Vendedor? vendedorExistenteComEsseEmail = contexto.Vendedores.FirstOrDefault(x=>x.Email==dto.Email && x.IsAtivo);
                    if (vendedorExistenteComEsseEmail != null)
                    {
                        return Results.BadRequest("Já existe um cliente cadastrado com esse e-mail!!");
                    }
                    Vendedor vendedorCriado = new Vendedor(dto.Nome, dto.Email, dto.Telefone);
                    contexto.Vendedores.Add(vendedorCriado);
                    contexto.SaveChanges();
                    return Results.Ok(vendedorCriado);
                }
                catch(Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });
            app.MapPut("/vendedores/{id}", (string id,VendedorDto dto, ContextoVenda contexto) =>
            {
                try
                {

                    if (string.IsNullOrWhiteSpace(dto.Nome))
                    {
                        return Results.BadRequest("O nome não deve ser vazio");
                    }
                    if (dto.Nome.Length < 3)
                    {
                        return Results.BadRequest("O nome do vendedor deve ter no mínimo 3 caracteres!!");
                    }
                    if (string.IsNullOrWhiteSpace(dto.Email))
                    {
                        return Results.BadRequest("O Email não deve ser vazio");
                    }
                    if (!Regex.IsMatch(dto.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    {
                        return Results.BadRequest("O Email deve vir em um formato válido");
                    }
                    Guid idConvertido;
                    if (!Guid.TryParse(id, out idConvertido))
                    {
                        return Results.BadRequest("id no formato inválido de GUID");
                    }
                    Vendedor? vendedorExistenteComEsseEmail = contexto.Vendedores.FirstOrDefault(x => x.Email == dto.Email && x.IsAtivo);
                    if (vendedorExistenteComEsseEmail != null)
                    {
                        return Results.BadRequest("Já existe um cliente cadastrado com esse e-mail!!");
                    }
                    Vendedor? vendedorVindoDoBanco = contexto.Vendedores.FirstOrDefault(x => x.Id == idConvertido && x.IsAtivo);
                    if (vendedorVindoDoBanco == null)
                    {
                        return Results.NotFound("Vendedor não Encontrado!!");
                    }
                    vendedorVindoDoBanco.Nome = dto.Nome;
                    vendedorVindoDoBanco.Email = dto.Email;
                    vendedorVindoDoBanco.Telefone = dto.Telefone;
                    contexto.Vendedores.Update(vendedorVindoDoBanco);
                    contexto.SaveChanges();
                    return Results.Ok(vendedorVindoDoBanco);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            app.MapDelete("/vendedores/{id}", (string id, ContextoVenda contexto) =>
            {
                try
                {
                    Guid idConvertido;
                    if (!Guid.TryParse(id, out idConvertido))
                    {
                        return Results.BadRequest("id no formato inválido de GUID");
                    }
                    Vendedor? vendedorVindoDoBanco = contexto.Vendedores.FirstOrDefault(x => x.Id == idConvertido && x.IsAtivo);
                    if (vendedorVindoDoBanco == null)
                    {
                        return Results.NotFound("Vendedor não Encontrado!!");
                    }
                    vendedorVindoDoBanco.Inativar();
                    contexto.Vendedores.Update(vendedorVindoDoBanco);
                    contexto.SaveChanges();

                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });


            app.Run();
        }
    }
}
