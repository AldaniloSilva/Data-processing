<h5> Data Structure Work </h5>
<h6>2018 – semester 3 - Teacher Eduardo Rosalém Marcelino</h6>
 
Develop a program in C #, which meets the requirements below. No data entry is required.
 
File structure:
 <ul>
<li>Categorias.txt

code | description</li>
 
<li>Products.txt

code | price | description | category | datacadastro in yyyyMMddHHmmss format</li>
 
<li>Clientes.txt

cpf | name</li>
 
<li>Vendas.txt

Sale code | customer | product | dataSale in format (" yyyyMMddHHmmss ") </li> 
 <ul>
 
Process the text files and extract the information listed below.
At the end of the processing, this information must be saved in a text file, the first character of each line of the file being the opCode (operation code). 
 
Conditions:
 <ul>
<li>Product valid is one that has a valid category;</li>
<li>A valid sale is one that has a valid product and a valid customer;</li>
<li>All operations must be performed only valid records, except when prompted different;</li>
<li>The opcodes must be written in alphabetical order in the file;</li>
<li>Decimal value fields must be formatted only with the decimal separator “,”;</li>
<li>File name: .txt resultado, should be saved in the \ bin \ debug folder;</li>
<li>In the result file, the first line should be the hour / min / sec of the start of processing in the HH format : mm: ss;</li>
<li>Result File Name:  Resultado.txt</li>
<li>The penultimate line of the file must be the hour / min / sec of the end of processing in the format; in HH format : mm: ss;</li>
<li>The last line of the file must be the time in seconds to process;</li>
<li>If there are records with repeated code, the first one that is imported is valid, and the rest must be discarded;</li>
<li>The total time of your program must take into account the time to load the text files;</li>
<li>The use of lambda and linq expressions is prohibited.</li>
<li>To do so, you must remove references from the following namespaces: using System.Linq;</li>
<ul>

OpCodes :
 
A. Total number of categories<br>
B. Total quantity of registered products<br>
C. Total number of customers<br>
D. Different quantity of individual sales (without repeating the sales code)<br>
E. Different quantity of products sold (without repeating the product code)<br>
F. Number of repeated customer names.<br>
G. Total amount of sales per customer (only for customers who made a purchase), one per line, ordered by CPF. EX:<br>
CPF | customer name1 | Total<br>
CPF | customer name2 | Total<br>
H. Quantity and total sum of sales per product, one per line, ordered by product name. EX:<br>
product name | code | Quantity | Total<br>
I. Total sales value (R $) per category, one per line, ordered by category description. Ex:<br>
category name | code | Total<br>
J. Total sales value (R $) per month / year, one per line, ordered by month / year. Ex:<br>
11/2017 | Total<br>
12/2017 | Total<br>
K. The customer who bought the most (R $). EX:<br>
customer name | value.<br>
If there is a tie, show all, one per line<br>
L. The most sold product (sales quantity) and the sum of its sales. EX:<br>
product name | value.<br>
If there is a tie, show all, one per line<br>
M. The month / year with the most sales (in R $ values). EX:<br>
month / year | value. If there is a tie, show all, one per line<br>
N. The highest value sale. EX:<br>
Sales code | Customer | total.<br>
If there is a tie, show them all, one per line.<br>
O. The quantity of products that are not included in any sale<br>
P. The number of customers that are not included in any sales<br>
Q. The number of categories that have no products sold. <br>
 

 
Result EX.txt.txt<br>
 
08:17:03<br>
A - 15<br>
...<br>
G - 123.456.789-09 | Ana maria | 500.30<br>
G - 123.897.336-01 | Silvio Santos | 1350.33<br>
...<br>
K - Ana maria | 632.22<br>
 
08: 1 8 : 0 0<br>
57
