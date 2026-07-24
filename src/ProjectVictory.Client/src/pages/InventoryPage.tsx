import { useEffect, useState } from "react";
import { getInventory } from "../services/characterService";

interface InventoryPageProps
{
    characterId: string;
}

export default function InventoryPage(
    {
        characterId
    }: InventoryPageProps
)
{
    const [items, setItems] = useState<string[]>([]);

    useEffect(() =>
    {
        async function loadInventory()
        {
            const result = await getInventory(characterId);

            setItems(result.items);
        }

        loadInventory();

    }, [characterId]);


    return (
        <div>
            <h1>Inventory</h1>

            {
                items.length === 0
                ?
                <p>Inventory vazio</p>
                :
                <ul>
                    {
                        items.map((item, index) =>
                            <li key={index}>
                                {item}
                            </li>
                        )
                    }
                </ul>
            }
        </div>
    );
}