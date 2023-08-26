# HenryMedsAPI

## About

Created with Microsoft's asp.net web api template.
Postgres running in docker (see below on how to run)
Using EF Core as an ORM
Swagger docs should open in a browser window for you when running the project

## Evaluation (From the Code Challenge Description)

- Does the code solve the business problem? ✅
- What trade-offs were made, how wise are they?
  - For the sake of time, even though I wanted to I tried to not focus too much on setting up a project that would be highly repeatable and run locally for the interviewers easily and has some test to prove the code does what it says it does. Initially I wanted to create tests, have linting, format the swagger docs nicely or provide a template for postman use, have a great seeded db right from the jump, but I ran into many issues during setup with postgres, ef core and the swagger docs that just zapped the time from this challenge.
  - I would like to think in a real world scenario, I would have working examples of similar projects that my team has used and I can leverage those to get jump started quickly and not need to make boatload of set up decisions.
  - I have included a list of TODOs below that I made as I went in order to not get distracted from ultimately getting the code working.
  - Initially I had wanted to build the appointments to be a little more flexible on the 15 minute time which is why an Appointment has a list of Availabilities. So that a user could reserve 30 minutes. I ran out of time to implement this completely.
- How clean/well structured is the code?
  - I think it's ok. Enough to get by, but not consistent. I would like to add in a linter and do pass for consistency.
- What ‘extra’ factors are there, that show exceptional talent?
  - Initially I had thought I might pop a 'real quick' UI on here as wll and use a blazor wasm template. That took too much time to set up and as the clock ticked on, I began to cut more corners.
  - Additionally I wanted to have this running in docker with a nice seed file for the interviewers, but sadly I ran out of time.

## Naming Considerations / Assumptions

- Client's don't really have reservations in the context of doctors visits, they have appointments. Reservations are for dinner or Native American land or indicate hesitations, appointments are for medical professionals. Appointments is the language of this domain so I've made the choice to change the naming.
- Length of appointments may change, but they will never be less than 15 minutes. If someone needs to book 30-45 minutes that can book multiple 'slots' reserved by the same person. (Some time later: I came to regret this forward looking assumption as the clock ticked away on this challenge)
- Similarly, I believe Providers actually submit their availability, not a schedule. The schedule can be the list of confirmed bookings, and because my assumption is that with this particular kind of business that does not have a front desk, these providers are not hourly employees, they won't be working a "scheduled shift." This assumption could be wrong, but I think still leaving some flexibility here for providers to submit availability and later receive a confirmed schedule could be a useful distinction.

### TOOD

- The database needs to handle things like setting the time of the record created automatically
- Add more constraints into db - out of time
- fix all the ids that are just integers for ease of this demo project to be GUIDs or a distinct id value for each entity (so that we don't accidentally send around an appointment id when we need an availability id)
- Seeding of the db needs to be handled better
- Organize the project structure a little more to anticipate many more files that we're seeing.
- Ensure a consistent approach in the controller actions
- Decide on error handling, if the db context isn't available we should ideally know when starting the app, not when getting a request
- Address all warnings
- Do a once over and decide are we doing everything async, get consistent at a minimum
- Fix the swagger documentation so that it's more clear what the flow of requesting an appointment and confirming one
- Consider a cron job or something else to remove requested appointments that have expired - unless the product should in fact allow a customer to reserve an appointment past the expiration. Currently we're just filtering on the appointment status and the expiration time field.
- Create the shared contact entity that both providers and clients will use (address, etc.)
- Add in auth, didn't enable that in the template for ease of sharing/postman etc.
- Consider splitting up request and response objects, some request shouldn't be sending in Ids for a request.
- Make the enum for appointment status human readable - 0 isn't great to get back from the api.

## Docker-compose

You'll need to have docker running already. If so:
Run the postgres db with docker compose `docker compose up -d`
And throw everything away when you're done with `docker compose down -v`. This will delete everything in the volumes.

### Does it work?

Yes, we can create clients, providers, enter the availability of a provider and a client can request an available appointment slot, which can then be confirmed.

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
