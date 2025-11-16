# LocationAPI

Location api izpostavi endpoint /car. Ta se pogovarja predvsem z aplikacijo. Za veè informacij o endpointu lahko pogledamo /swagger. 

## Namestitev

Najlažje lahko aplikacijo namestimo tako zaženemo njen Dockerfile. Ko jo zaženemo moramo nastavit nekaj okoljskih spremenljivk:

- ASPNETCORE_HTTPS_PORTS : port kjer gostimo https npr. 443
- ASPNETCORE_HTTP_PORTS : port kjer gostimo http npr. 80
- TIMESCALE_CONN_STRING : povezava do podatkovne baze (\* podatkovna baza ne rabi biti timescale, ampak je bilo planirano kot da je)