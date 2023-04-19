import requests
import os
from bs4 import BeautifulSoup


download_types = ["pokemon-sprites", "items"]  # The possibilities of downloads
downloads = ["items"]  # The downloads to perform

# Set the path to save the images
path = "pokemon-downloads"
# Set the generation and versions to download
version = "black-white"

# Create the necessary directories
# For the pokemon sprites
os.makedirs(f"{path}/pokemon-sprites/{version}/unanimated/shiny/front", exist_ok=True)
os.makedirs(f"{path}/pokemon-sprites/{version}/unanimated/shiny/back", exist_ok=True)
os.makedirs(f"{path}/pokemon-sprites/{version}/unanimated/normal/front", exist_ok=True)
os.makedirs(f"{path}/pokemon-sprites/{version}/unanimated/normal/back", exist_ok=True)
os.makedirs(f"{path}/pokemon-sprites/{version}/animated/shiny/front", exist_ok=True)
os.makedirs(f"{path}/pokemon-sprites/{version}/animated/shiny/back", exist_ok=True)
os.makedirs(f"{path}/pokemon-sprites/{version}/animated/normal/front", exist_ok=True)
os.makedirs(f"{path}/pokemon-sprites/{version}/animated/normal/back", exist_ok=True)
os.makedirs(f"{path}/pokemon-sprites/icons", exist_ok=True)

# For the items
os.makedirs(f"{path}/items", exist_ok=True)

if "pokemon-sprites" in downloads:
    # Fetch the webpage and parse it using BeautifulSoup
    url = "https://pokemondb.net/sprites"
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

        sprite_urls = [
            src,
            f"https://img.pokemondb.net/sprites/{version}/shiny/{name}.png",
            f"https://img.pokemondb.net/sprites/{version}/back-shiny/{name}.png",
            f"https://img.pokemondb.net/sprites/{version}/normal/{name}.png",
            f"https://img.pokemondb.net/sprites/{version}/back-normal/{name}.png",
            f"https://img.pokemondb.net/sprites/{version}/anim/shiny/{name}.gif",
            f"https://img.pokemondb.net/sprites/{version}/anim/back-shiny/{name}.gif",
            f"https://img.pokemondb.net/sprites/{version}/anim/normal/{name}.gif",
            f"https://img.pokemondb.net/sprites/{version}/anim/back-normal/{name}.gif",
        ]

        sprite_file_names = [
            f"{path}/pokemon-sprites/icons/{num}.png",
            f"{path}/pokemon-sprites/{version}/unanimated/shiny/front/{num}.png",
            f"{path}/pokemon-sprites/{version}/unanimated/shiny/back/{num}.png",
            f"{path}/pokemon-sprites/{version}/unanimated/normal/front/{num}.png",
            f"{path}/pokemon-sprites/{version}/unanimated/normal/back/{num}.png",
            f"{path}/pokemon-sprites/{version}/animated/shiny/front/{num}.gif",
            f"{path}/pokemon-sprites/{version}/animated/shiny/back/{num}.gif",
            f"{path}/pokemon-sprites/{version}/animated/normal/front/{num}.gif",
            f"{path}/pokemon-sprites/{version}/animated/normal/back/{num}.gif",
        ]

        for url, fname in zip(sprite_urls, sprite_file_names):
            r = requests.get(url)
            if not r.ok:
                continue
            with open(fname, "wb") as f:
                f.write(r.content)

if "items" in downloads:
    url = "https://pokemondb.net/item/all"
    response = requests.get(url)

    soup = BeautifulSoup(response.text, "html.parser")

    # Find all the image tags on the page
    img_tags = soup.find_all("img")

    # Loop through all the image tags
    for img in img_tags:
        src = img["src"]
        item_name = src.split("/")[-1].split(".")[0]

        # Remove the items that do not have sprites
        if item_name == "s":
            continue

        # Get the URL for the item sprite
        item_url = f"https://img.pokemondb.net/sprites/items/{item_name}.png"

        # Get the file name for the item sprite
        item_file_name = f"{path}/items/{item_name}.png"

        # Download the item sprite
        r = requests.get(item_url)
        if not r.ok:
            continue
        with open(item_file_name, "wb") as f:
            f.write(r.content)
