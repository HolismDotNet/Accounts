curl \
  -d "client_id=admin-cli" \
  -d "username=admin_user" \
  -d "password=admin_password" \
  -d "grant_type=password" \
  "https://accounts.freya.center/auth/realms/master/protocol/openid-connect/token"

  curl \
  -H "Authorization: bearer token" \
  "https://accounts.freya.center/auth/admin/realms/master/users"