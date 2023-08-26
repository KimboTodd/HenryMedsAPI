# HenryMedsAPI

## About

Created with Microsoft's asp.net web api template.
Postgres running in docker (see below on how to run)
Using EF Core as an ORM

## Evaluation (From the Code Challenge Description)

- Does the code solve the business problem? ✅
- What trade-offs were made, how wise are they?
  - For the sake of time, I tried to not focus too much on setting up a project that would be highly repeatable and testable. Initially I wanted to create tests, have linting, format the swagger docs nicely or provide a template for postman use, have a great seeded db right from the jump, but I ran into many issues during setup with postgres and ef core that just zapped the time from this challenge.
  - I would like to think in a real world scenario, I would have working examples of similar projects that my team has used and we can leverage those to get jumpstarted quickly to not need to make boatload of set up decisions.
  - I have included a list of TODOs below that I made as I went in order to not get distracted from ultimately getting the code working.
- How clean/well structured is the code?
  - I think it's ok. Enough to get by, but not consistent.
- What ‘extra’ factors are there, that show exceptional talent?

### TOOD

- database needs to handle things like setting the time of the record created atutomaticaly
- add more constraints into db
- fix all the ids that are just int for east of this demo project to be GUIDs or a distinct id value for each entity (so that we don't accidentally send around an appointment id when we need an availability id)
- seeding of the db needs to be handled better
- organize folders a little better, ensure a consistent approach
- decide on error handling, if the db context isn't available we should ideally know when starting the app
- handle all warnings
- do a once over and decide are we doing everything async, get consistent at a minimum
- fix the swagger documentation so that it's more clear the flow of requesting an appointment and confirming one
- consider a cron job or something else to remove requested appointments that have expired - unless the product should in fact allow a customer to reserve an appointment past the expiration.
- create the shared contact entity that both providers and clients will use (address, etc.)
- add in auth, didn't enable that in the template for ease of sharing/postman etc.
- consider splitting up request and response objects, some request shouldn't be sending in Ids for a request.
- Make the enum for appointment status human readable - 0 isn't great to get back from the api.

## Naming Considerations


```
curl -X 'PUT' \
  'http://localhost:5180/api/Appointment?appointmentId=3' \
  -H 'accept: */*'
Request URL
http://localhost:5180/api/Appointment?appointmentId=3
Server response
Code	Details
200
Response body
Download
{
  "id": 3,
  "availabilities": null,
  "client": null,
  "provider": null,
  "status": 1,
  "created": "2023-08-26T18:55:03.57465Z",
  "requestExpires": "2023-08-26T19:25:03.574751Z"
}
Response headers
 content-type: application/json; charset=utf-8
 date: Sat,26 Aug 2023 18:55:34 GMT
 server: Kestrel
 transfer-encoding: chunked
```

## Docker-compose

You'll need to have docker running already. If so:
Run the postgres db with docker compose `docker compose up -d`
And throw everything away when you're done with `docker compose down -v`. This will delete everything in the volumes.
