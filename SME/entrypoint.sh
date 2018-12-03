#!/bin/bash
set -e
run_cmd="dotnet run"
./wait-for-it.sh -t 0 rabbitmq:15672 -- echo "rabbitmq is up"
>&2 echo "RabbitMQ is Up"
exec $run_cmd
