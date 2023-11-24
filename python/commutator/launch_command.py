import yaml
import subprocess
import sys


#For now this python command_launch.py config.yaml command_query.py


def create_seq_command_from_yaml(yaml_file):
    with open(yaml_file, 'r') as file:
        config = yaml.safe_load(file)

    seq_commands = []
    args = []

    # Check each key and build the command
    for key, value in config.items():
        if value:
            seq_commands.append(key[0])  # Assuming the first letter is the command

            # Handle multiple contents
            if key == 'content' and isinstance(value, list):
                for content in value:
                    args.append(f'--{key} "{content}"')
            else:
                args.append(f'--{key} "{value}"')

    return ' '.join(args), '|'.join(seq_commands)

def main():
    if len(sys.argv) != 3:
        print("Usage: python launch_command_from_yaml.py <yaml_file> <python_script>")
        sys.exit(1)

    yaml_file = sys.argv[1]
    python_script = sys.argv[2]

    args, seq = create_seq_command_from_yaml(yaml_file)
    full_command = f'python {python_script} {args} -seq "{seq}"'

    # Execute the Python script with the constructed command
    subprocess.run(full_command, shell=True)

if __name__ == "__main__":
    main()
