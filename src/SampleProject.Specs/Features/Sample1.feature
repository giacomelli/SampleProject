#language: pt-br
Funcionalidade: Exemplo 1
	COMO usuário
	QUERO acessar a página de exemplo
	PARA demonstrar o uso do SpecFlow
	 
Contexto:          
	Quando acesso a página principal

Cenário: Páginal principal
	Então deve exibir o texto 'ASP.NET is a free web framework'

	Quando clico no link 'About'
	Então deve exibir o texto 'Your application description page'

	Quando clico no link 'Contact'
	Então deve exibir o texto 'Your contact page'

	Quando clico no link 'Register'
	Então deve exibir o texto 'Create a new account'
	Quando clico no botão 'Register new'
	Então deve exibir o texto 'The Email field is required'
	E deve exibir o texto 'The Password field is required'
	
	Quando digito 'test@sampleproject.com' no campo 'Email'
	Quando clico no botão 'Register new'
	Então não deve exibir o texto 'The Email field is required'
	E deve exibir o texto 'The Password field is required'
	
	Quando digito 'abc' no campo 'Password'
	Quando clico no botão 'Register new'
	Então não deve exibir o texto 'The Email field is required'
	E não deve exibir o texto 'The Password field is required'
	E deve exibir o texto 'The Password must be at least 6 characters long'
	E deve exibir o texto 'The password and confirmation password do not match'

