import requests
import json

def process_tree(tree_json):
    tree_data = json.loads(tree_json)

    for key, value in tree_data.items():
        print(f"{key}: {value}")

def process_result(result_json):
    result_data = json.loads(result_json)

    for key, value in result_data.items():
        tree = value.get('tree')
        evaluation = value.get('evaluation')

        print(f"{key}:")
        print("  Tree:")
        process_tree(tree)
        print(f"  Evaluation: {evaluation}")

def process_response(response_data):
    action = response_data.get('action')

    if action == 'REGISTERCLIENT':
        print("REGISTERCLIENT action:")
    elif action == 'UNREGISTERCLIENT':
        print("UNREGISTERCLIENT action:")
    elif action == 'GETPARAMETERS':
        print("GETPARAMETERS action:")
    elif action == 'GETDATA':
        print("GETDATA action:")
    elif action == 'CLIENTGETJOB':
        print("CLIENTGETJOB action:")
    elif action == 'CROSSOVER':
        print("CROSSOVER action:")
        result_json = response_data['result']
        process_result(result_json)
    elif action == 'MUTATION':
        print("MUTATION action:")
    elif action == 'CREATETREES':
        print("CREATETREES action:")
    elif action == 'GETATTRIBUTESTEXT':
        print("GETATTRIBUTESTEXT action:")
    elif action == 'GETATTRIBUTES':
        print("GETATTRIBUTES action:")
    elif action == 'BUSY':
        print("BUSY action:")
    else:
        print(f"Unhandled action: {action}")
        print("Response data:", response_data)

def send_post_request(url, data, headers):
    response = requests.post(url, data=json.dumps(data), headers=headers)

    if response.status_code == 200:
        print("POST request was successful.")
        response_data = response.json()
        process_response(response_data)
    else:
        print(f"Error {response.status_code}: {response.text}")

def main():
    url = "http://localhost:29999/api/cycle"
    data = {
        "datarunID": "30",
        "action": "CLIENTGETJOB"
    }
    headers = {
        'Content-Type': 'application/json'
    }
    send_post_request(url, data, headers)

if __name__ == "__main__":
    main()
