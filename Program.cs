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
                "Port=3306;" +
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
                try
                {
                    return Results.Ok(contexto.Clientes.Where(c=>c.IsAtivo).ToList());
                }
                catch(Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });
            app.MapGet("/clientes/{id}", (string id, ContextoVenda contexto) =>
            {

                try
                {
                    Guid idClienteConvertido;
                    if (!Guid.TryParse(id, out idClienteConvertido))
                    {
                        return Results.BadRequest("id do cliente no formato inválido de GUID");
                    }
                    Cliente? clienteVindoDoBanco = contexto.Clientes.FirstOrDefault(c => c.Id == idClienteConvertido && c.IsAtivo);
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
                    Guid idClienteConvertido;
                    if (!Guid.TryParse(id, out idClienteConvertido))
                    {
                        return Results.BadRequest("id do cliente no formato inválido de GUID");
                    }
                    Cliente? clienteVindoDoBanco = contexto.Clientes.FirstOrDefault(c => c.Id == idClienteConvertido && c.IsAtivo);
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
                    Guid idClienteConvertido;
                    if (!Guid.TryParse(id, out idClienteConvertido))
                    {
                        return Results.BadRequest("id do cliente no formato inválido de GUID");
                    }
                    Cliente? clienteVindoDoBanco = contexto.Clientes.FirstOrDefault(c => c.Id == idClienteConvertido && c.IsAtivo);
                    if (clienteVindoDoBanco == null)
                    {
                        return Results.NotFound("Cliente não Encontrado!!");
                    }
                    clienteVindoDoBanco.Inativar();
                    clienteVindoDoBanco.AtualizarDataAtualizacao();
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
                try
                {
                    return Results.Ok(contexto.Produtos.Where(p=>p.IsAtivo).ToList());
                }
                catch(Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            app.MapGet("/produtos/{id}", (string id, ContextoVenda contexto) =>
            {
                try
                {
                    Guid idProdutoConvertido;
                    if (!Guid.TryParse(id, out idProdutoConvertido))
                    {
                        return Results.BadRequest("id do produto no formato inválido de GUID");
                    }
                    Produto? produtoVindoDoBanco = contexto.Produtos.FirstOrDefault(p => p.Id == idProdutoConvertido && p.IsAtivo);
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
                    Guid idProdutoConvertido;
                    if (!Guid.TryParse(id, out idProdutoConvertido))
                    {
                        return Results.BadRequest("id do produto no formato inválido de GUID");
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
                    Produto? produtoVindoDoBanco = contexto.Produtos.FirstOrDefault(p => p.Id == idProdutoConvertido && p.IsAtivo);
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
                    Guid idProdutoConvertido;
                    if (!Guid.TryParse(id, out idProdutoConvertido))
                    {
                        return Results.BadRequest("id do produto no formato inválido de GUID");
                    }
                    Produto? produtoVindoDoBanco = contexto.Produtos.FirstOrDefault(c => c.Id == idProdutoConvertido && c.IsAtivo);
                    if (produtoVindoDoBanco == null)
                    {
                        return Results.NotFound("Produto não Encontrado!!");
                    }
                    produtoVindoDoBanco.Inativar();
                    produtoVindoDoBanco.AtualizarDataAtualizacao();
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
                try
                {
                    return Results.Ok(contexto.Vendas.Include(v=>v.Cliente).Include(v=>v.Vendedor).Include(v=>v.Itens.Where(i=>i.IsAtivo)).ThenInclude(i=>i.Produto).Where(v=>v.IsAtivo).ToList());
                }
                catch(Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            app.MapGet("/vendas/{id}", (string id, ContextoVenda contexto) =>
            {
                try
                {
                    Guid idVendaConvertido;
                    if (!Guid.TryParse(id, out idVendaConvertido))
                    {
                        return Results.BadRequest("id da venda no formato inválido de GUID");
                    }
                    Venda? vendaVindoDoBanco = contexto.Vendas.Include(v=>v.Cliente).Include(v=>v.Vendedor).Include(v=>v.Itens.Where(i=>i.IsAtivo)).ThenInclude(i=>i.Produto).FirstOrDefault(v => v.Id == idVendaConvertido && v.IsAtivo);
                    if (vendaVindoDoBanco == null)
                    {
                        return Results.NotFound("Venda não Encontrada!!");
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
            {//validar guid
                try
                {

                    if (string.IsNullOrWhiteSpace(dto.IdCliente))
                    {
                        return Results.BadRequest("O id do cliente não pode ser vazio");
                    }

                    if (string.IsNullOrWhiteSpace(dto.IdVendedor))
                    {
                        return Results.BadRequest("O id do vendedor não pode ser vazio");
                    }

                    Guid idClienteConvertido;
                    if (!Guid.TryParse(dto.IdCliente, out idClienteConvertido))
                    {
                        return Results.BadRequest("id do cliente no formato inválido de GUID");
                    }

                    Guid idVendedorConvertido;
                    if (!Guid.TryParse(dto.IdVendedor, out idVendedorConvertido))
                    {
                        return Results.BadRequest("id do vendedor no formato inválido de GUID");
                    }


                    Cliente? cliente = contexto
                        .Clientes
                        .FirstOrDefault(c => c.Id == idClienteConvertido && c.IsAtivo);

                    if (cliente == null)
                        return Results.NotFound("Cliente não Encontrado!");

                    Vendedor? vendedor = contexto
                       .Vendedores
                       .FirstOrDefault(v => v.Id == idVendedorConvertido && v.IsAtivo);

                    if (vendedor == null)
                        return Results.NotFound("Vendedor não Encontrado!");

                    Venda vendaCriada = new Venda(idClienteConvertido,cliente,idVendedorConvertido,vendedor);

                    contexto.Vendas.Add(vendaCriada);
                    contexto.SaveChanges();

                    vendaCriada.Itens=vendaCriada.Itens.Where(i => i.IsAtivo).ToList();

                    return Results.Created($"/vendas/{vendaCriada.Id}", vendaCriada);
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

                    if (string.IsNullOrWhiteSpace(dto.IdCliente))
                    {
                        return Results.BadRequest("O id do cliente não pode ser vazio");
                    }

                    if (string.IsNullOrWhiteSpace(dto.IdVendedor))
                    {
                        return Results.BadRequest("O id do vendedor não pode ser vazio");
                    }

                    Guid idClienteConvertido;
                    if (!Guid.TryParse(dto.IdCliente, out idClienteConvertido))
                    {
                        return Results.BadRequest("id do cliente no formato inválido de GUID");
                    }

                    Guid idVendedorConvertido;
                    if (!Guid.TryParse(dto.IdVendedor, out idVendedorConvertido))
                    {
                        return Results.BadRequest("id do vendedor no formato inválido de GUID");
                    }

                    Cliente? cliente = contexto
                        .Clientes
                        .FirstOrDefault(c => c.Id == idClienteConvertido && c.IsAtivo);

                    if (cliente == null)
                        return Results.NotFound("Cliente não Encontrado!");

                    Vendedor? vendedor = contexto
                       .Vendedores
                       .FirstOrDefault(v => v.Id == idVendedorConvertido && v.IsAtivo);

                    if (vendedor == null)
                        return Results.NotFound("Vendedor não Encontrado!");

                    Guid idVendaConvertido;
                    if (!Guid.TryParse(id, out idVendaConvertido))
                    {
                        return Results.BadRequest("id da venda no formato inválido de GUID");
                    }
                    Venda? vendaVindaDoBanco = contexto.Vendas.Include(v=>v.Cliente).Include(v=>v.Vendedor).Include(v=>v.Itens).FirstOrDefault(x => x.Id == idVendaConvertido && x.IsAtivo);
                    if (vendaVindaDoBanco == null)
                    {
                        return Results.NotFound("Venda não Encontrada!!");
                    }
                    vendaVindaDoBanco.AtualizarDataAtualizacao();
                    vendaVindaDoBanco.ClienteId = idClienteConvertido;
                    vendaVindaDoBanco.Cliente= cliente;
                    vendaVindaDoBanco.VendedorId = idVendedorConvertido;
                    vendaVindaDoBanco.Vendedor = vendedor;
                    contexto.Vendas.Update(vendaVindaDoBanco);
                    contexto.SaveChanges();
                    vendaVindaDoBanco.Itens= vendaVindaDoBanco.Itens.Where(i => i.IsAtivo).ToList();
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
                    Guid idVendaConvertido;
                    if (!Guid.TryParse(id, out idVendaConvertido))
                    {
                        return Results.BadRequest("id da venda no formato inválido de GUID");
                    }
                    Venda? vendaVindoDoBanco = contexto.Vendas.Include(v=>v.Itens).FirstOrDefault(v => v.Id == idVendaConvertido && v.IsAtivo);
                    if (vendaVindoDoBanco == null)
                    {
                        return Results.NotFound("Venda não Encontrada!!");
                    }
                    vendaVindoDoBanco.Inativar();
                    vendaVindoDoBanco.AtualizarDataAtualizacao();
                    foreach(ItemVenda item in vendaVindoDoBanco.Itens.ToList())
                    {
                        item.Inativar();
                        contexto.ItensVenda.Update(item);
                    }
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

                    Guid idVendaConvertido;
                    if (!Guid.TryParse(idVenda, out idVendaConvertido))
                    {
                        return Results.BadRequest("id da venda no formato inválido de GUID");
                    }
                    Venda? venda = contexto
                        .Vendas
                            .Include(v => v.Cliente)
                            .Include(v=>v.Vendedor)
                            .Include(v => v.Itens)
                                .ThenInclude(i => i.Produto)
                            .FirstOrDefault(v => v.Id == idVendaConvertido && v.IsAtivo);

                    if (venda == null)
                        return Results.NotFound("Venda não Encontrada!");

                    if (string.IsNullOrWhiteSpace(dto.IdProduto))
                    {
                        return Results.BadRequest("O id do produto não pode ser vazio");
                    }

                    Guid idProdutoConvertido;
                    if (!Guid.TryParse(dto.IdProduto, out idProdutoConvertido))
                    {
                        return Results.BadRequest("id do produto no formato inválido de GUID");
                    }

                    Produto? produto = contexto
                        .Produtos
                        .FirstOrDefault(p => p.Id == idProdutoConvertido && p.IsAtivo);

                    if (produto == null)
                        return Results.NotFound("Produto não Encontrado!");

                    if (dto.Quantidade <= 0.0m)
                        return Results.BadRequest("A quantidade do produto deve ser maior que 0!");

                    if (produto.Estoque < dto.Quantidade )
                        return Results.BadRequest("Não há estoque suficiente para venda!");

                    produto.AbaterEstoque(dto.Quantidade);

                    ItemVenda itemVenda = new ItemVenda(produto, dto.Quantidade);

                    venda.AdicionarItem(itemVenda);

                    contexto.ItensVenda.Add(itemVenda);
                    contexto.Produtos.Update(produto);
                    contexto.Vendas.Update(venda);

                    contexto.SaveChanges();

                    venda.Itens=venda.Itens.Where(i => i.IsAtivo).ToList();

                    return Results.Created($"vendas/{venda.Id}", venda);
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
                    Guid idVendaConvertido;
                    Guid idItemVendaConvertido;

                    if(!Guid.TryParse(idVenda, out idVendaConvertido))
                    {
                        return Results.BadRequest("id da venda no formato inválido de GUID");
                    }
                    if (!Guid.TryParse(idItem, out idItemVendaConvertido))
                    {
                        return Results.BadRequest("id do item venda no formato inválido de GUID");
                    }

                    Venda? venda = contexto
                        .Vendas
                        .Include(v => v.Cliente)
                        .Include(v=>v.Vendedor)
                        .Include(v => v.Itens)
                            .ThenInclude(i => i.Produto)
                        .FirstOrDefault(v => v.Id == idVendaConvertido && v.IsAtivo);

                    if (venda == null)
                        return Results.NotFound("Venda não Encontrada!");

                    ItemVenda? itemVenda = venda
                        .Itens
                        .FirstOrDefault(x => x.Id == idItemVendaConvertido && x.IsAtivo);

                    if (itemVenda == null)
                        return Results.NotFound("Item de venda não Encontrado");

                    itemVenda.Produto.AdicionarEstoque(itemVenda.Quantidade);
                    venda.RemoverItem(itemVenda);

                    contexto.Produtos.Update(itemVenda.Produto);
                    contexto.ItensVenda.Remove(itemVenda);
                    contexto.Vendas.Update(venda);

                    contexto.SaveChanges();

                    venda.Itens=venda.Itens.Where(i => i.IsAtivo).ToList();

                    return Results.Ok(venda);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            app.MapGet("/vendedores", (ContextoVenda contexto) =>
            {
                try
                {
                    return Results.Ok(contexto.Vendedores.Where(x=>x.IsAtivo).ToList());
                }
                catch(Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            app.MapGet("/vendedores/{id}", (string id, ContextoVenda contexto) =>
            {
                try
                {
                    Guid idVendedorConvertido;
                    if (!Guid.TryParse(id, out idVendedorConvertido))
                    {
                        return Results.BadRequest("id do vendedor no formato inválido de GUID");
                    }
                    Vendedor? vendedorVindoDoBanco = contexto.Vendedores.FirstOrDefault(v => v.Id == idVendedorConvertido && v.IsAtivo);
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
                    if (string.IsNullOrWhiteSpace(dto.Telefone))
                    {
                        dto.Telefone = "";
                    }
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
                    Vendedor? vendedorExistenteComEsseEmail = contexto.Vendedores.FirstOrDefault(v=>v.Email==dto.Email && v.IsAtivo);
                    if (vendedorExistenteComEsseEmail != null)
                    {
                        return Results.BadRequest("Já existe um vendedor cadastrado com esse e-mail!!, o e-mail deve ser unico!!");
                    }
                    Vendedor vendedorCriado = new Vendedor(dto.Nome, dto.Email, dto.Telefone);
                    contexto.Vendedores.Add(vendedorCriado);
                    contexto.SaveChanges();
                    return Results.Created($"/vendedores/{vendedorCriado.Id}",vendedorCriado);
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
                    if (string.IsNullOrWhiteSpace(dto.Telefone))
                    {
                        dto.Telefone = "";
                    }
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
                    Guid idVendedorConvertido;
                    if (!Guid.TryParse(id, out idVendedorConvertido))
                    {
                        return Results.BadRequest("id do vendedor no formato inválido de GUID");
                    }
                    Vendedor? vendedorExistenteComEsseEmail = contexto.Vendedores.FirstOrDefault(v => v.Email == dto.Email && v.Id!=idVendedorConvertido && v.IsAtivo);
                    if (vendedorExistenteComEsseEmail != null)
                    {
                        return Results.BadRequest("Já existe um vendedor cadastrado com esse e-mail!!, o e-mail deve ser unico");
                    }
                    Vendedor? vendedorVindoDoBanco = contexto.Vendedores.FirstOrDefault(v => v.Id == idVendedorConvertido && v.IsAtivo);
                    if (vendedorVindoDoBanco == null)
                    {
                        return Results.NotFound("Vendedor não Encontrado!!");
                    }
                    vendedorVindoDoBanco.AtualizarDataAtualizacao();
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
                    Guid idVendedorConvertido;
                    if (!Guid.TryParse(id, out idVendedorConvertido))
                    {
                        return Results.BadRequest("id do vendedor no formato inválido de GUID");
                    }
                    Vendedor? vendedorVindoDoBanco = contexto.Vendedores.FirstOrDefault(v => v.Id == idVendedorConvertido && v.IsAtivo);
                    if (vendedorVindoDoBanco == null)
                    {
                        return Results.NotFound("Vendedor não Encontrado!!");
                    }
                    vendedorVindoDoBanco.AtualizarDataAtualizacao();
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
