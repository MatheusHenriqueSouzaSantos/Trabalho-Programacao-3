using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                return contexto.Clientes.ToList();
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

            app.MapPost("/vendas", ([FromBody] TransacaoDTO.TransacaoCapaRequest dto,
                ContextoVenda contexto) =>
            {
                try
                {
                    Cliente? cliente = contexto
                        .Clientes
                        .FirstOrDefault(x => x.Id == dto.IdCliente && x.IsAtivo);

                    if (cliente == null)
                        throw new InvalidOperationException("Cliente não cadastrado!");

                    Venda venda = new Venda(cliente);

                    contexto.Vendas.Add(venda);
                    contexto.SaveChanges();

                    return Results.Created($"vendas/{venda.Id}", venda);
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
                        throw new InvalidOperationException("Venda não cadastrada!");

                    Produto? produto = contexto
                        .Produtos
                        .FirstOrDefault(x => x.Id == dto.IdProduto && x.IsAtivo);

                    if (produto == null)
                        throw new InvalidOperationException("Produto não cadastrado!");

                    if (dto.Quantidade <= 0.0m)
                        throw new InvalidOperationException("Quantidade informada inválida!");

                    if (produto.Estoque - dto.Quantidade < 0.0m)
                        throw new InvalidOperationException("Não há estoque suficiente para venda!");

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
                        throw new InvalidOperationException("Venda não cadastrada!");

                    ItemVenda? itemVenda = venda
                        .Itens
                        .FirstOrDefault(x => x.Id == idItemVendaConvertido && x.IsAtivo);

                    if (itemVenda == null)
                        throw new InvalidOperationException("Item de venda não cadastrado");

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
                        return Results.NotFound("Vendedore não Encontrado!!");
                    }
                    return Results.Ok(vendedorVindoDoBanco);

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
