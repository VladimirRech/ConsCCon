import random
import string

def generate_cnpj():
  return ''.join(random.choice(string.digits) for _ in range(14))

def generate_company_name():
  words = ['Empresa', 'Fictícia', 'Comercial', 'Industrial', 'Ltda', 'SA']
  return ' '.join(random.choice(words) for _ in range(3))
  
def generate_company_state():
  words = ['PR', 'SP', 'SC', 'RS', 'MG', 'RJ', 'BA']
  return ' '.join(random.choice(words) for _ in range(1))
  

with open('cnpjs.csv', 'w') as f:
  for _ in range(1000):
    cnpj = generate_cnpj()
    company_name = generate_company_name()
    state = generate_company_state()
    f.write(f'{cnpj};{company_name};{state}\n')
