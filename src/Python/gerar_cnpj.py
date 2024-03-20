import random
import string

def generate_cnpj():
  return ''.join(random.choice(string.digits) for _ in range(14))

def generate_company_name():
  words = ['Empresa', 'Fictícia', 'Comercial', 'Industrial', 'Ltda', 'SA']
  return ' '.join(random.choice(words) for _ in range(3))

with open('new_file.csv', 'w') as f:
  for _ in range(1000):
    cnpj = generate_cnpj()
    company_name = generate_company_name()
    f.write(f'{cnpj};{company_name}\n')
