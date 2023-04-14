import requests
import os
from bs4 import BeautifulSoup
import pprint

# Set the URL to download from
url = "https://pokemondb.net/sprites"

# Set the path to save the images
path = "pokemon-sprites"

# Set the generation and versions to download
version = "black-white"

# Create the necessary directories
os.makedirs(f"{path}/{version}/shiny/front", exist_ok=True)
os.makedirs(f"{path}/{version}/shiny/back", exist_ok=True)
os.makedirs(f"{path}/{version}/normal/front", exist_ok=True)
os.makedirs(f"{path}/{version}/normal/back", exist_ok=True)
os.makedirs(f"{path}/icons", exist_ok=True)

# Fetch the webpage and parse it using BeautifulSoup
response = requests.get(url)
soup = BeautifulSoup(response.text, "html.parser")

# Find all the image tags on the page
img_tags = soup.find_all("img")

# Loop through all the image tags
for i, img in enumerate(img_tags[1:]):
    # Get the source URL for the icon
    src = img["src"]

    # Get the name of the Pokemon from the URL
    name = src.split("/")[-1].split(".")[0]
    # Get the National Pokedex number for the Pokemon
    num = i + 1

    # Download the icon
    r = requests.get(src)
    with open(f"{path}/icons/{num}.png", "wb") as f:
        f.write(r.content)

    # Download the shiny front sprite
    r = requests.get(f"https://img.pokemondb.net/sprites/{version}/shiny/{name}.png")
    with open(f"{path}/{version}/shiny/front/{num}.png", "wb") as f:
        f.write(r.content)
    # Download the shiny back sprite
    r = requests.get(
        f"https://img.pokemondb.net/sprites/{version}/back-shiny/{name}.png"
    )
    with open(f"{path}/{version}/shiny/back/{num}.png", "wb") as f:
        f.write(r.content)
    # Download the non-shiny front sprite
    r = requests.get(f"https://img.pokemondb.net/sprites/{version}/normal/{name}.png")
    with open(f"{path}/{version}/normal/front/{num}.png", "wb") as f:
        f.write(r.content)
    # Download the non-shiny back sprite
    r = requests.get(
        f"https://img.pokemondb.net/sprites/{version}/back-normal/{name}.png"
    )
    with open(f"{path}/{version}/normal/back/{num}.png", "wb") as f:
        f.write(r.content)
