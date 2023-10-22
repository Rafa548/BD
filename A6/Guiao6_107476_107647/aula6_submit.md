# BD: Guião 6

## Problema 6.1

### *a)* Todos os tuplos da tabela autores (authors);

```
    USE pubs;

    SELECT * FROM authors;
```

### *b)* O primeiro nome, o último nome e o telefone dos autores;

```
    USE pubs;

    SELECT au_fname,au_lname,phone FROM authors;
```

### *c)* Consulta definida em b) mas ordenada pelo primeiro nome (ascendente) e depois o último nome (ascendente); 

```
    USE pubs;

    SELECT au_fname,au_lname,phone FROM authors
    ORDER BY au_fname ASC,au_lname ASC;
```

### *d)* Consulta definida em c) mas renomeando os atributos para (first_name, last_name, telephone); 

```
    USE pubs;

    SELECT au_fname AS fist_name,au_lname AS last_name,phone AS telephone FROM authors
    ORDER BY au_fname ASC,au_lname ASC;

```

### *e)* Consulta definida em d) mas só os autores da Califórnia (CA) cujo último nome é diferente de ‘Ringer’; 

```
    USE pubs;

    SELECT au_fname AS fist_name,au_lname AS last_name,phone AS telephone FROM authors
    WHERE state='CA'
    ORDER BY au_fname ASC,au_lname ASC;
```

### *f)* Todas as editoras (publishers) que tenham ‘Bo’ em qualquer parte do nome; 

```
    Use Pubs

    Select * From publishers
    Where pub_name Like '%_Bo_%'
```

### *g)* Nome das editoras que têm pelo menos uma publicação do tipo ‘Business’; 

```
    Use Pubs

    Select Distinct pub_name From publishers Join titles ON (publishers.pub_id = titles.pub_id)
    Where type='business'
```

### *h)* Número total de vendas de cada editora; 

```
    Use Pubs

    Select pub_name,Sum(ytd_sales) as n_vendas From publishers Join titles ON (publishers.pub_id = titles.pub_id)
    Group by pub_name
```

### *i)* Número total de vendas de cada editora agrupado por título; 

```
    Use Pubs

    Select pub_name,title,Sum(ytd_sales) as n_vendas From publishers Join titles ON (publishers.pub_id = titles.pub_id)
    Group by pub_name,title
```

### *j)* Nome dos títulos vendidos pela loja ‘Bookbeat’; 

```
    USE pubs;

    SELECT title FROM sales JOIN stores ON (sales.stor_id=stores.stor_id) JOIN titles ON (sales.title_id=titles.title_id) 
    WHERE stor_name='Bookbeat'; 
```

### *k)* Nome de autores que tenham publicações de tipos diferentes; 

```
    USE pubs;

    SELECT  DISTINCT  au_fname,au_lname FROM titles JOIN titleauthor ON (titles.title_id=titleauthor.title_id) JOIN authors ON (titleauthor.au_id=authors.au_id)
    GROUP BY au_fname, au_lname
    HAVING count(DISTINCT type) > 1;
```

### *l)* Para os títulos, obter o preço médio e o número total de vendas agrupado por tipo (type) e editora (pub_id);

```
    USE pubs;

    SELECT title,type,AVG(price) as Price,ytd_sales,titles.pub_id FROM titles JOIN sales ON (titles.title_id=sales.title_id) JOIN publishers ON (titles.pub_id=publishers.pub_id)
    GROUP BY title,type,ytd_sales,titles.pub_id
```

### *m)* Obter o(s) tipo(s) de título(s) para o(s) qual(is) o máximo de dinheiro “à cabeça” (advance) é uma vez e meia superior à média do grupo (tipo);

```
    Use pubs

    Select type  From titles
    Group by type
    Having max(advance) > 1.5 * avg(advance)
```

### *n)* Obter, para cada título, nome dos autores e valor arrecadado por estes com a sua venda;

```
    Use pubs

    Select title,au_fname,au_lname,price * ytd_sales * royalty/100 * royaltyper/100 as au_profit From titles 
    Join titleauthor on titles.title_id = titleauthor.title_id
    Join authors on titleauthor.au_id = authors.au_id
    where price is not null

```

### *o)* Obter uma lista que incluía o número de vendas de um título (ytd_sales), o seu nome, a faturação total, o valor da faturação relativa aos autores e o valor da faturação relativa à editora;

```
    Use pubs

    Select Distinct title,ytd_sales,ytd_sales*price as facturacao,price * ytd_sales * royalty/100 * royaltyper/100 as auths_revenue,(ytd_sales*price) - (price * ytd_sales * royalty/100 * royaltyper/100) as publisher_revenue 
    From titles 
    Join titleauthor on titles.title_id = titleauthor.title_id
    where ytd_sales is not null
```

### *p)* Obter uma lista que incluía o número de vendas de um título (ytd_sales), o seu nome, o nome de cada autor, o valor da faturação de cada autor e o valor da faturação relativa à editora;

```
    Use pubs

    Select Distinct title,ytd_sales,au_fname+' '+au_lname as author,price * ytd_sales * royalty/100 * royaltyper/100 as auths_revenue,(ytd_sales*price) - (price * ytd_sales * royalty/100 * royaltyper/100) as publisher_revenue 
    From titles 
    Join titleauthor on titles.title_id = titleauthor.title_id
    Join authors on titleauthor.au_id = authors.au_id
    where ytd_sales is not null
```

### *q)* Lista de lojas que venderam pelo menos um exemplar de todos os livros;

```
    Use pubs

    Declare @max_books Int

    Select @max_books = Count(Distinct titles.title_id)
    From titles
    Join sales on titles.title_id=sales.title_id
    Join stores on stores.stor_id=sales.stor_id

    Select stores.stor_name,sales.stor_id,Count(Distinct titles.title_id) as n_livros
    From titles
    Join sales on titles.title_id=sales.title_id
    Join stores on stores.stor_id=sales.stor_id
    Group by stores.stor_name,sales.stor_id
    having Count(Distinct titles.title_id) = @max_books
```

### *r)* Lista de lojas que venderam mais livros do que a média de todas as lojas;

```
    USE pubs;

    SELECT stor_name,SUM(qty) AS [Quantity] FROM stores JOIN sales ON (stores.stor_id=sales.stor_id)
    GROUP BY stor_name
    HAVING sum(qty)>(SELECT AVG(total_qty) FROM (SELECT SUM(qty) as total_qty FROM sales GROUP BY stor_id) as stor_sales)
```

### *s)* Nome dos títulos que nunca foram vendidos na loja “Bookbeat”;

```
    USE pubs;

    SELECT title FROM titles
    EXCEPT
    SELECT title FROM stores  JOIN sales ON (stores.stor_id=sales.stor_id) RIGHT JOIN titles ON (sales.title_id=titles.title_id)
    WHERE stor_name='Bookbeat'
```

### *t)* Para cada editora, a lista de todas as lojas que nunca venderam títulos dessa editora; 

```
    USE pubs;

    SELECT DISTINCT stores.stor_name, publishers.pub_name
    FROM stores CROSS JOIN publishers LEFT JOIN (titles LEFT JOIN sales ON titles.title_id = sales.title_id ) ON publishers.pub_id = titles.pub_id AND sales.stor_id = stores.stor_id
    WHERE sales.qty IS NULL
```

## Problema 6.2

### ​5.1

#### a) SQL DDL Script
 
[a) SQL DDL File](ex_6_2_1_ddl.sql "SQLFileQuestion")

#### b) Data Insertion Script

[b) SQL Data Insertion File](ex_6_2_1_data.sql "SQLFileQuestion")

#### c) Queries

##### *a)*

```
    Select Pname,Ssn,Fname,Minit,Lname From employee Join  works_on on (Ssn = Essn) Join Project on (Pno = Pnumber)
```

##### *b)* 

```
    Declare @c_ssn Int

    Select @c_ssn = Ssn From employee 
    Where Fname = 'Carlos' and Lname = 'Gomes'

    Select Fname,Minit,Lname From employee 
    Where Super_Ssn = @c_ssn
```

##### *c)* 

```
    Select Pname,Sum(Hours) as N_horas From works_on Join Project on (Pno = Pnumber)
    Group By Pname
```

##### *d)* 

```
    Select Fname,Minit,Lname From works_on Join Project on (Pno = Pnumber) Join employee on (Ssn = Essn)
    Where Dnum = 3 and Hours > 20 and Pname = 'Aveiro Digital'
    Group By Fname,Minit,Lname
```

##### *e)* 

```
    Select Fname,Minit,Lname From employee left outer Join works_on on (Essn=Ssn)
    Where Pno is null
```

##### *f)* 

```
    Select Dname,Avg(Salary) as sal_med_f From department Join employee on (Dno = Dnumber)
    Where Sex = 'F'
    Group By Dname
```

##### *g)* 

```
    Select Fname,Minit,Lname,Count(Dependent_name) as D_num From employee Join dependent on (Essn = Ssn)
    Group by Fname,Minit,Lname
    Having Count(Dependent_name) > 2

```

##### *h)* 

```
    Select Fname,Minit,Lname From department Join employee on (Ssn = Mgr_ssn) left outer Join dependent on (Ssn=Essn)
    Where Dependent_name is null
```

##### *i)* 

```
    Select Distinct Fname,Minit,Lname,Adress From employee Join works_on on (Essn = Ssn) Join Project on (Pno = Pnumber) Join department on (Dnumber = Dnum) Join dept_locations on (department.Dnumber = dept_locations.Dnumber)
    Where Plocation = 'Aveiro' and Dlocation != 'Aveiro'
```

### 5.2

#### a) SQL DDL Script
 
[a) SQL DDL File](ex_6_2_2_ddl.sql "SQLFileQuestion")

#### b) Data Insertion Script

[b) SQL Data Insertion File](ex_6_2_2_data.sql "SQLFileQuestion")

#### c) Queries

##### *a)*

```
    SELECT nome FROM fornecedor FULL OUTER JOIN encomenda ON (fornecedor.nif=encomenda.fornecedor)
    WHERE numero IS NULL
```

##### *b)* 

```
    SELECT codProd,AVG(unidades)  FROM item
    GROUP BY codProd
```


##### *c)* 

```
    SELECT numEnc,AVG(unidades)  FROM item
    GROUP BY numEnc
```


##### *d)* 

```
    SELECT fornecedor,codProd,item.unidades,nome FROM item LEFT OUTER JOIN encomenda ON (item.numEnc=encomenda.numero) JOIN produto ON (item.codProd=produto.codigo)
```

### 5.3

#### a) SQL DDL Script
 
[a) SQL DDL File](ex_6_2_3_ddl.sql "SQLFileQuestion")

#### b) Data Insertion Script

[b) SQL Data Insertion File](ex_6_2_3_data.sql "SQLFileQuestion")

#### c) Queries

##### *a)*

```
    Select nome From Paciente left outer Join Prescricao on (Paciente.numUtente = Prescricao.numUtente)
    Where numPresc is null
```

##### *b)* 

```
    Select especialidade,Count(especialidade) as n_presc_espe From Medico Join Prescricao on (Medico.numSns = Prescricao.numMedico)
    Group By especialidade
```


##### *c)* 

```
    Select nome,count(nome) as n_presc_farm From Farmacia Join Prescricao on (Farmacia.nome = Prescricao.farmacia)
    Group by nome
```


##### *d)* 

```
    Select farmaco.numRegFarm,nome From farmaco left outer Join presc_farmaco on (nome=nomeFarmaco)
    Where farmaco.numRegFarm = 906 and nomeFarmaco is null
```

##### *e)* 

```
    Select nome,numRegFarm,Count(nome) From farmacia Join Prescricao on (farmacia = nome) Join presc_farmaco on (presc_farmaco.numPresc=Prescricao.numPresc)
    Group by nome,numRegFarm

```

##### *f)* 

```
    SELECT Paciente.numUtente, Paciente.nome,COUNT(DISTINCT numSns) as n_medicos
    FROM Paciente JOIN prescricao ON (Paciente.numUtente = prescricao.numUtente) JOIN medico ON (numMedico = numSns)
    GROUP BY Paciente.numUtente,Paciente.nome
    Having COUNT(DISTINCT numSns)>1
```
