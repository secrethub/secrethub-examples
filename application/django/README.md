Init the SecretHub demo repo with example values
```
secrethub demo init
```

Create a service account for the demo repo
```
secrethub service init --description demo_service \
--permission read --file demo_service.cred <your_username>/demo
```

Build the sinatra docker demo
```
docker build . -t django:demo
```

Run the docker demo with the secrets in the environment variables
```
docker run -ti -p 8080:8000 \
  -e DEMO_USERNAME=secrethub://<your_username>/demo/username \
  -e DEMO_PASSWORD=secrethub://<your_username>/demo/password \
  -e SECRETHUB_CREDENTIAL=$(cat demo_service.cred) \
  django:demo
```