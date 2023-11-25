import yaml
import subprocess
import sys

def create_seq_command_from_yaml(yaml_file):
    with open(yaml_file, 'r', encoding='utf-8-sig') as file:
        config = yaml.safe_load(file)

    args = []
    seq = config.get('seq', '')  # Get the sequence string directly

    for key, value in config.items():
        if key == 'seq':
            continue  # Skip the 'seq' key since it's handled separately
        if isinstance(value, list):
            for item in value:
                args.append(f'--{key} "{item}"')
        elif isinstance(value, bool):
            if value:
                args.append(f'--{key}')
        else:
            args.append(f'--{key} "{value}"')

    return ' '.join(args), seq

def main():
    default_python_script = 'command_query.py'  # Set the default script name

    if len(sys.argv) < 2:
        print(f"Usage: python launch_command_from_yaml.py <yaml_file1> <yaml_file2> ... [python_script]")
        print(f"If no python_script is specified, {default_python_script} will be used.")
        sys.exit(1)

    if len(sys.argv) > 2 and sys.argv[-1].endswith('.py'):
        python_script = sys.argv[-1]  
        yaml_files = sys.argv[1:-1]
    else:
        python_script = default_python_script
        yaml_files = sys.argv[1:]

    for yaml_file in yaml_files:
        args, seq = create_seq_command_from_yaml(yaml_file)
        full_command = f'python {python_script} {args} -seq "{seq}"'  # Correctly formatted command
        print(f"Executing: {full_command}")
        subprocess.run(full_command, shell=True)

if __name__ == "__main__":
    main()
