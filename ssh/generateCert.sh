#!/usr/bin/env bash

subj_base="/C=US/ST=TN/L=Nashville/O=Batte/OU=Examples/CN="

###
 # Check for root name.
 ##
root=$1
shift
if [[ -z "$root" ]]; then
  echo "you must pass 2 arguments; first for root name, second for child name"
  exit
fi

###
 # Check for child name
 ##
child=$1
if [[ -z "$child" ]]; then
  echo "you must pass 2 arguments; first for root name ($root), second for child name"
  exit
fi

###
 # Use the child name as the password
 ##
pw="$child"

###
 # Generate the root certificate
 ##
root_key="$root.key"
root_pem="$root.pem"
root_subj="$subj_base$root\_ca"

###
 # Generate the root private key
 ##
if [[ -e "$root_key" ]]; then
  echo "$root_key already exits"
else
  echo "generate $root_key"
  openssl genrsa -aes256 -passout pass:"$pw" -out "$root_key" 4096
fi

###
 # Generate the the root privacy enhanced email (PEM)
 ##
if [[ -e "$root_pem" ]]; then
  echo "$root_pem already exits"
else
  echo "generate $root_pem"
  openssl req -new -x509 -days 3652 -key "$root_key" -out "$root_pem" -passin pass:"$pw" -subj "$root_subj"
fi

###
 # Generate the child certificate
 ##
child_name="${root}_${child}"
child_key="$child_name.key"
child_pem="$child_name.pem"
child_csr="$child_name.csr"
child_subj="$subj_base$child_name"
child_p12="$child_name.p12"

###
 # Generate the child private key
 ##
if [[ -e "$child_key" ]]; then
  echo "$child_key already exits"
else
  echo "generate $child_key"
  openssl genrsa -aes256 -passout pass:"$pw" -out "$child_key" 4096
fi

###
 # Generate the the child privacy enhanced email (PEM)
 ##
if [[ -e "$child_pem" ]]; then
  echo "$child_pem already exits"
else
  echo "generate $child_csr"
  openssl req -new -key "$child_key" -passin pass:"$pw" -out "$child_csr" -subj "$child_subj"
  echo "generate $child_pem"
  openssl x509 -req -days 36524 -in "$child_csr" -CA "$root_pem" -CAkey "$root_key" -passin pass:"$pw" -set_serial 1 \
    -out "$child_pem"
fi

###
 # Generate the child public key (P12)
 ##
if [[ -e "$child_p12" ]]; then
  echo "$child_p12 already exits"
else
  echo "generate $child_p12"
  openssl pkcs12 -export -in "$child_pem" -inkey "$child_key" -passin pass:"$pw" -passout pass:"$pw" -out "$child_p12" \
    -certfile "$root_pem" -caname "$root" -name "$child_name"
fi
