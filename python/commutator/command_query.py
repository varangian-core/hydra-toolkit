import json
import requests
import argparse
import os

# Functions to handle arguments
messages = []
chat_content = ''

def handle_model(model_name):
    print(f"Model name: {model_name}")

def handle_content(content):
    global chat_content, messages
    user_message = {"role": "user", "content": content}
    messages.append(user_message)
    chat_content += "User: " + content + "\n"

    data = {
        "messages": messages,
        "mode": "instruct",  # or "chat" depending on the context
        "model": args.model
    }

    try:
        response = requests.post(url, headers=headers, json=data, verify=False)
        response_data = response.json()

        if response_data and 'choices' in response_data and len(response_data['choices']) > 0:
            assistant_message = response_data['choices'][0]['message']['content']
            if not args.suppress:
                print(assistant_message)
            messages.append({"role": "assistant", "content": assistant_message})
            if not args.clean:
                with open(chat_history_file, 'w') as file:
                    json.dump(messages, file)

            if args.output:
                with open(args.output, 'w', encoding='utf-8') as file:
                    file.write("User: " + chat_content + "\nAssistant: " + assistant_message)
        else:
            if not args.suppress:
                print("No response received.")
    except requests.RequestException as e:
        print(f"Error communicating with the AI service: {e}")

def handle_buffer(buffer_path):
    global chat_content
    with open(buffer_path, 'r', encoding='utf-8') as file:
        chat_content += file.read().strip()

def handle_output(output_path):
    global chat_content
    try:
        with open(output_path, 'w', encoding='utf-8') as file:
            file.write(chat_content)
        print(f"Output written to {output_path}")
    except Exception as e:
        print(f"Error writing to file: {e}")

def handle_suppress(suppress):
    global suppress_output
    suppress_output = suppress

def handle_clean(clean):
    global messages
    if clean and os.path.exists(chat_history_file):
        os.remove(chat_history_file)
    messages = []

def handle_website_content(url):
    try:
        response = requests.get(url)
        response.raise_for_status()
        return response.text
    except requests.RequestException as e:
        print(f"Error fetching url: {e}")
        return None

def handle_website(url):
    global chat_content
    scraped_content = handle_website_content(url)
    print("[DEBUG] Scraped content: " + (scraped_content if scraped_content else "None"))
    if scraped_content:
        chat_content += " "
    chat_content += scraped_content or ''

# Mapping argument names to functions
argument_functions = {
    'model': handle_model,
    'content': handle_content,
    'buffer': handle_buffer,
    'output': handle_output,
    'suppress': handle_suppress,
    'clean': handle_clean,
    'website': handle_website,
}

# Set up argument parser
parser = argparse.ArgumentParser(description='Generate chat completions based on a series of messages.')
parser.add_argument('--model', type=str, default='TheBloke_WizardCoder-Python-34B-V1.0-GPTQ', help='Model name for completion')
parser.add_argument('-c', '--content', action='append', help='Chat message content')
parser.add_argument('-b', '--buffer', type=str, help='File path for reading chat message content')
parser.add_argument('-o', '--output', type=str, help='File path for writing the current prompt and response')
parser.add_argument('-s', '--suppress', action='store_true', help='Suppress printing of the output to the terminal')
parser.add_argument('--clean', action='store_true', help='Clear the chat history')
parser.add_argument('-w', '--website', type=str, help='URL of the website to scrape')
parser.add_argument('--sequence', '-seq', type=str, help="Execute a sequence of commands")
args = parser.parse_args()

# Define the API endpoint and headers
url = "http://127.0.0.1:5000/v1/chat/completions"
headers = {"Content-Type": "application/json"}

# File to store chat history
chat_history_file = 'chat_history.json'

# Handle the clean flag or load chat history
if args.clean:
    if os.path.exists(chat_history_file):
        os.remove(chat_history_file)
    messages = []
else:
    if os.path.exists(chat_history_file):
        with open(chat_history_file, 'r', encoding='utf-8') as file:
            messages = json.load(file)
    else:
        messages = []

# Execute functions based on the --seq argument
content_index = 0
if args.sequence:
    seq_commands = args.sequence.split('|')
    for command in seq_commands:
        if command == 'c' and args.content and content_index < len(args.content):
            handle_content(args.content[content_index])
            content_index += 1
        elif command == 'b' and args.buffer:
            handle_buffer(args.buffer)
        elif command == 'w' and args.website:
            handle_website(args.website)
        elif command == 'o' and args.output:
            handle_output(args.output)
        elif command == 's' and args.suppress:
            handle_suppress(args.suppress)
        elif command == 'clean' and args.clean:
            handle_clean(args.clean)
        elif command == 'model' and args.model:
            handle_model(args.model)
        else:
            print(f"Unknown command: {command}")
            break

# Handle arguments if --seq is not used
if not args.sequence:
    if args.website:
        handle_website(args.website)
    if args.buffer:
        handle_buffer(args.buffer)
    if args.content:
        for item in args.content:
            handle_content(item)

    # Make POST request if there is content to process
    if chat_content:
        data = {
            "messages": messages,
            "mode": "instruct",  # or "chat" depending on the context
            "model": args.model
        }

        response = requests.post(url, headers=headers, json=data, verify=False)
        response_data = response.json()

        if response_data and 'choices' in response_data and len(response_data['choices']) > 0:
            assistant_message = response_data['choices'][0]['message']['content']
            if not args.suppress:
                print(assistant_message)
            messages.append({"role": "assistant", "content": assistant_message})
            if not args.clean:
                with open(chat_history_file, 'w', encoding='utf-8') as file:
                    json.dump(messages, file)
            if args.output:
                with open(args.output, 'w', encoding='utf-8') as file:
                    file.write("User: " + chat_content + "\nAssistant: " + assistant_message)
        else:
            if not args.suppress:
                print("No response received.")
