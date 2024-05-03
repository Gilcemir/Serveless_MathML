build-no-cache:
	cd src && docker build --no-cache -t mathml .

build:
	cd src && docker build -t mathml .

run-image:
	docker run -p 8080:8080 mathml

test-lambda-locally:
	sam local start-api
	
using-current-port:
	sudo lsof -i -P -n | grep 3000 # or any other port

kikll-port:
	sudo kill -9 <PID>