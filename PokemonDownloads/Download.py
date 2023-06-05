import requests
import os
from bs4 import BeautifulSoup


download_types = ["pokemon-sprites", "items", "stats"]  # The possibilities of downloads
downloads = ["stats"]  # The downloads to perform

# Set the path to save the images
path = "pokemon-downloads"

if "pokemon-sprites" in downloads:
    # Set the generation and versions to download
    version = "black-white"

    # Create the necessary directories
    os.makedirs(f"{path}/pokemon-sprites/{version}/unanimated/shiny/front", exist_ok=True)
    os.makedirs(f"{path}/pokemon-sprites/{version}/unanimated/shiny/back", exist_ok=True)
    os.makedirs(f"{path}/pokemon-sprites/{version}/unanimated/normal/front", exist_ok=True)
    os.makedirs(f"{path}/pokemon-sprites/{version}/unanimated/normal/back", exist_ok=True)
    os.makedirs(f"{path}/pokemon-sprites/{version}/animated/shiny/front", exist_ok=True)
    os.makedirs(f"{path}/pokemon-sprites/{version}/animated/shiny/back", exist_ok=True)
    os.makedirs(f"{path}/pokemon-sprites/{version}/animated/normal/front", exist_ok=True)
    os.makedirs(f"{path}/pokemon-sprites/{version}/animated/normal/back", exist_ok=True)
    os.makedirs(f"{path}/pokemon-sprites/icons", exist_ok=True)

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
    # Create the necessary directories
    os.makedirs(f"{path}/items", exist_ok=True)

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

if "stats" in downloads:
    url = "https://pokemondb.net/pokedex/all"
    response = requests.get(url)
    soup = BeautifulSoup(response.text, "html.parser")
    table = soup.find("table", {"id": "pokedex"})
    rows = table.find("tbody").find_all("tr")
    with open("pokemon-stats.txt", "w") as f:
        for idx, row in enumerate(rows):
            pokemon_number = row.find("span", {"class": "infocard-cell-data"}).text
            pokemon_name = row.find("a", {"class": "ent-name"}).text
            is_main_pokemon = row.find("a", {"class": "ent-name"}).parent.find("small") is None
            if not is_main_pokemon:
                print(f"""Skipping {pokemon_name} ({row.find("a", {"class": "ent-name"}).parent.find("small").text})""")
                continue
            pokemon_types = row.find_all("a", {"class": "type-icon"})
            types_text = "Types: " + " ".join(x.text for x in pokemon_types)
            x = ""

            # Additional information
            url = "https://pokemondb.net/pokedex/" + pokemon_name.lower()
            response = requests.get(url)
            soup = BeautifulSoup(response.text, "html.parser")

            species_found = soup.find("th", string="Species") is not None
            if not species_found:
                print(f"""Skipping {pokemon_name} (no species found)""")
                continue

            species = soup.find("th", string="Species").find_next_sibling("td").text
            height_str = soup.find("th", string="Height").find_next_sibling("td").text
            weight_str = soup.find("th", string="Weight").find_next_sibling("td").text
            height_list = height_str.split(" ")
            weight_list = weight_str.split(" ")
            height = height_list[0]
            weight = weight_list[0]

            abilities = soup.find("th", string="Abilities").find_next_sibling("td").find_all("a")
            abilities_text = " / ".join(x.text for x in abilities)

            hp = soup.find("th", string="HP").find_next_sibling("td").text
            atq = soup.find("th", string="Attack").find_next_sibling("td").text
            defe = soup.find("th", string="Defense").find_next_sibling("td").text
            spa = soup.find("th", string="Sp. Atk").find_next_sibling("td").text
            spd = soup.find("th", string="Sp. Def").find_next_sibling("td").text
            spe = soup.find("th", string="Speed").find_next_sibling("td").text
            total = soup.find("th", string="Total").find_next_sibling("td").text

            ev_yield = soup.find("th", string="EV yield").find_next_sibling("td").text
            ev_yield = ev_yield.replace("Special Attack", "SpA")
            ev_yield = ev_yield.replace("Special Defense", "SpD")
            ev_yield = ev_yield.replace("Attack", "Atq")
            ev_yield = ev_yield.replace("Defense", "Def")
            ev_yield = ev_yield.replace("Speed", "Spe")
            ev_yield = ev_yield.strip()

            catch_rate = soup.find("th", string="Catch rate").find_next_sibling("td").text.split(" ")[0].strip()
            base_friendship = (
                soup.find("a", string="Friendship").parent.find_next_sibling("td").text.split(" ")[0].strip()
            )
            base_exp = soup.find("th", string="Base Exp.").find_next_sibling("td").text
            growth_rate = soup.find("th", string="Growth Rate").find_next_sibling("td").text
            gender = soup.find("th", string="Gender").find_next_sibling("td").text

            evolution_msg = "None"
            evolution_chart = soup.find("h2", string="Evolution chart").find_next_sibling(
                "div", {"class": "infocard-list-evo"}
            )
            if evolution_chart is not None:
                evolution_stage = -1
                evolution_cards = evolution_chart.find_all("div", {"class": "infocard"})
                n_stages = len(evolution_cards)
                for i in range(n_stages - 1):
                    if evolution_cards[i].find("a", {"class": "ent-name"}).text == pokemon_name:
                        evolution_msg = f"{evolution_cards[i+1].find('a', {'class':'ent-name'}).text}"
                        evolution_stage = i
                        break
                if evolution_stage != -1:
                    evo_method_info = evolution_chart.find_all("span", {"class": "infocard-arrow"})[evolution_stage]
                    evo_method_text = evo_method_info.find("small").text
                    evolution_msg += f" {evo_method_text}"

            moves_info = (
                soup.find("h3", string="Moves learnt by level up")
                .find_next_sibling("div")
                .find("table")
                .find("tbody")
                .find_all("tr")
            )
            moves = []
            for info in moves_info:
                level = info.find("td", {"class": "cell-num"}).text
                move = info.find("a", {"class": "ent-name"}).text
                moves.append(f"{level} - {move}")
            moves_text = "\n".join(moves)

            f.write("====================\n")
            f.write(f"No. {pokemon_number} - {pokemon_name}\n")
            f.write("====================\n")
            f.write("\n")

            f.write(types_text + "\n")
            f.write("\n")

            f.write(f"Species: {species}\n")
            f.write(f"Height: {height}\n")
            f.write(f"Weight: {weight}\n")
            f.write("\n")

            f.write(f"Abilities: {abilities_text}\n")
            f.write("\n")

            f.write("Base stats\n")
            f.write(f"Old: HP {hp} / Atq {atq} / Def {defe} / SpA {spa} / SpD {spd} / Spe {spe} / Total {total}\n")
            f.write("New: \n")
            f.write("\n")

            f.write(f"EV yield: {ev_yield}\n")
            f.write(f"Catch rate: {catch_rate}\n")
            f.write(f"Base friendship: {base_friendship}\n")
            f.write(f"Base exp: {base_exp}\n")
            f.write(f"Growth rate: {growth_rate}\n")
            f.write(f"Gender: {gender}\n")
            f.write("\n")

            f.write(f"Evolution: {evolution_msg}\n")
            f.write("\n")

            f.write("Moves\n")
            f.write(moves_text)

            f.write("\n")
            f.write("\n")

            if idx == 45:
                break
