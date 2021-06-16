# SocialNetwork

Projeto de rede social baseada no framework ASP.NET Core e utilizando o Entity Framework.

A aplicação está organizada em quatro camadas:

1. Camada de apresentação ->
Contém os controllers que se comunicam com a API e repassam as informações para as views.

2. Camada de aplicação -> 
Possui exclusivamente os controllers de API que interagem com a camada de acesso ao repositório

3. Camada de acesso ao repositório(D.A.L) -> 
Esta camada é responsável por interagir com o repositório

4. Camada de negócio -> 
Apenas o código responsável pelas regras de negócio estão nesta camada, não é armazenada nenhuma informação.
