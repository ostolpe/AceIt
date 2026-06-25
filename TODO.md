# TODO

Refresh token - currently user has to relog every 30 minutes

1. login endpoint -> generate refresh token -> return TokenResponse -> save hash of refresh token to db
2. refresh endpoint -> find user in db from refresh token -> make new token+refresh token, invalidate old -> return TokenResponse
3. frontend calls refresh endpoint on 401

- Refresh on timer
- Refresh on failed req
- Invalidate token on multiple users

Fix so backend returns an equal amount of questions from each topic
Different modes of questions (weaknesses, general knowledge, specifics topics etc)
Easy/medium/hard-mode. Or junior/mid/senior mode?

## Security

- how can we assure users dont spend a million tokens? word cap on answers? rate limiting on submitting?
- not great with my own JWT - exchange for oAuth or similar (maybe only external login tbh, github+google should be enough)

## Ideas on func to implement

- show details for individual topics? click on one and see all of the questions/answers/feedback?
- show old sessions?
- allow users to print a "diploma" for of their results somehow?
- email sign up and reset password
- email reminder to do tests?
- progress tracking somehow - levels, average graph - not sure but everybody loves seeing meters go up over time
