from bs4 import BeautifulSoup
import requests 
import json

base_url = "https://www.fuelcellstore.com/fuel-cell-components/membranes?limit=100&page="
pages = 2 # usually no more than 200 items on the store
headers = {'User-Agent': "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.246"}

products = []

# fetch all products
for page in range(pages):
    print(f"Fetching page {page+1} of {pages}...")

    r = requests.get(url=base_url+str(page+1), headers=headers) 
    soup = BeautifulSoup(r.content, 'lxml')
    table = soup.find('div', attrs = {'class':'product-list'})

    for entry in table.find_all('div', attrs = {'class':''}):
        row = entry.find('div', attrs = {'class':'name'})
        name = row.a.text
        link = row.a['href']
        stock = entry.find('div', attrs = {'class':'price'}) != None

        product = {}
        product['name'] = name
        product['link'] = link
        product['stock'] = stock
        products.append(product)

print("Fetching product specs...")

# fetch info for each product
for product in products:
    print(f"Fetching specs for {product['name']}...")

    product_url = product['link']

    product['spec'] = {}

    r = requests.get(url=product_url, headers=headers) 
    soup = BeautifulSoup(r.content, 'lxml')
    table = soup.find('table', attrs = {'class':'attribute'})
    if table is not None:
        for entry in table.find('tbody').find_all('tr'):
            spec, value = entry.find_all('td')

            # clean spec for consistency
            # sometimes spec names have units, sometimes not
            formatted_spec = spec.text.split(" (")[0]

            product['spec'][formatted_spec] = value.text

# dump to json
with open('data/raw_dump.json', 'w') as out:
    print("Dumping product info...")
    json.dump(products, out, indent=4)

print("Done")
        
