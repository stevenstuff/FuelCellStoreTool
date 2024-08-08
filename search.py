import json
import re
import math
import difflib

# helper function to parse numerical specs as ranges
# returns between [0, inf]
def parse_numerical(str):

    nums = [float(s) for s in re.findall(r"[-+]?(?:\d*\.*\d+)", str)]

    if len(nums) >= 1:
        operator = str[0:1]
        if operator == '<':
            return [0, nums[0]]
        elif operator == '>':
            return [nums[0], math.inf]
        else:
            if len(nums) >= 2 and ('-' in str or 'to' in str):
                return [nums[0], nums[1]]
            else:
                return [nums[0], nums[0]]

    return [0, math.inf]

# load scraped data
with open('data/raw_dump.json', 'r') as file:

    products = json.load(file)
    print(f"Loaded {len(products)} products")

    # get set of all params
    params = {}
    for product in products:
        for param in product['spec'].keys():
            if param not in params:
                params[param] = "num" if any(char.isdigit() for char in product['spec'][param]) else "string"

    # input loop
    while True:
        spec_input = input("Enter a property to filter: ").upper()

        spec_match = None
        for param in params.keys():
            if spec_input in param.upper() and (spec_match is None or len(param) < len(spec_match)):
                spec_match = param

        if spec_match is None:
            if spec_input.upper() == 'HELP':
                print("Enter a property to filter by to begin")
                print("If property value is a string, enter desired value when prompted")
                print("If property value is a numerical value or range, enter desired value and operator when prompted")
                print("Valid operators include >, <, and =. Operator must be placed before the value. (Ex: >50)")
            elif spec_input.upper() == 'QUIT':
                print(f"Final matches: {len(products)}")
                for product in products:
                    print(f"{product['name']} - {product['link']}")
                exit()
            else:
                print("Property not found")
        else:
            value_type = params[spec_match]

            if value_type == "string":
                value_input = input(f"Enter string value for {spec_match}: ")

                if value_input.upper() == "QUIT":
                    continue

                products = [x for x in products if 
                            spec_match in x['spec'] and
                            value_input.upper() in x['spec'][spec_match].upper()]

            elif value_type == "num":
                value_input = input(f"Enter number value and operator for {spec_match}: ")

                if value_input.upper() == "QUIT":
                    continue

                operator = value_input[0:1]
                value = float(value_input[1::])

                if operator == '=':
                    products = [x for x in products if 
                                spec_match in x['spec'] and
                                value >= parse_numerical(x['spec'][spec_match])[0] and
                                value <= parse_numerical(x['spec'][spec_match])[1]]
                elif operator == '<':
                    products = [x for x in products if 
                                spec_match in x['spec'] and
                                parse_numerical(x['spec'][spec_match])[1] <= value]
                elif operator == '>':
                    products = [x for x in products if 
                                spec_match in x['spec'] and
                                parse_numerical(x['spec'][spec_match])[0] >= value]
                else:
                    print("Operator invalid")

            print(f"Filter applied to product list. Resulting matches: {len(products)}")

