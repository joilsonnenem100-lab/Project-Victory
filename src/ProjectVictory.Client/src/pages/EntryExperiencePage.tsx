// src/ProjectVictory.Client/src/pages/EntryExperiencePage.tsx

interface CharacterInfo
{
    id?: string;
    name?: string;
    level?: number;
    experience?: number;
    title?: string;
    titleTier?: number;
}

interface Props
{
    character?: CharacterInfo;
    onEnterWorld: () => void;
}

export default function EntryExperiencePage({ character, onEnterWorld }: Props)
{
    function handleClick()
    {
        console.log("Botão Enter World clicado!");
        if (typeof onEnterWorld === "function")
        {
            onEnterWorld();
        }
        else
        {
            console.error("Aviso: A função onEnterWorld não foi passada corretamente pelo componente pai.");
        }
    }

    return (
        <main
            style={{
                position: "fixed",
                inset: 0,
                width: "100vw",
                height: "100vh",
                backgroundColor: "#090a0f",
                backgroundImage: "radial-gradient(circle at center, #1a1f2c 0%, #090a0f 100%)",
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                fontFamily: "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif",
                color: "#e2e8f0",
                overflow: "hidden"
            }}
        >
            <div
                style={{
                    width: "100%",
                    maxWidth: "540px",
                    margin: "0 20px",
                    padding: "48px",
                    backgroundColor: "#131722",
                    border: "2px solid #30363d",
                    borderRadius: "16px",
                    boxShadow: "0 16px 40px rgba(0, 0, 0, 0.7)",
                    display: "flex",
                    flexDirection: "column",
                    gap: "28px",
                    textAlign: "center"
                }}
            >
                <div>
                    <h1 style={{ fontSize: "32px", margin: "0 0 10px 0", color: "#f0b429", letterSpacing: "1.5px", textTransform: "uppercase" }}>
                        World Entry
                    </h1>
                    <p style={{ fontSize: "15px", color: "#8b949e", margin: 0 }}>
                        Your character is ready to enter the world.
                    </p>
                </div>

                <div
                    style={{
                        backgroundColor: "#090a0f",
                        border: "1px solid #30363d",
                        borderRadius: "10px",
                        padding: "24px",
                        display: "flex",
                        flexDirection: "column",
                        gap: "14px",
                        textAlign: "left"
                    }}
                >
                    <strong style={{ fontSize: "15px", color: "#f0b429", borderBottom: "1px solid #30363d", paddingBottom: "10px", letterSpacing: "0.5px" }}>
                        Character Context
                    </strong>
                    <div style={{ display: "flex", justifyContent: "space-between", fontSize: "14px", color: "#8b949e" }}>
                        <span>Name:</span>
                        <span style={{ color: "#fff", fontWeight: "bold" }}>{character?.name || "Unknown"}</span>
                    </div>
                    <div style={{ display: "flex", justifyContent: "space-between", fontSize: "14px", color: "#8b949e" }}>
                        <span>Character ID:</span>
                        <span style={{ color: "#fff", fontWeight: "bold", fontSize: "12px" }}>{character?.id || "N/A"}</span>
                    </div>
                    <div style={{ display: "flex", justifyContent: "space-between", fontSize: "14px", color: "#8b949e" }}>
                        <span>Level:</span>
                        <span style={{ color: "#fff", fontWeight: "bold" }}>{character?.level ?? 1}</span>
                    </div>
                    <div style={{ display: "flex", justifyContent: "space-between", fontSize: "14px", color: "#8b949e" }}>
                        <span>Title:</span>
                        <span style={{ color: "#fff", fontWeight: "bold" }}>{character?.title ?? "-"}</span>
                    </div>
                    <div style={{ display: "flex", justifyContent: "space-between", fontSize: "14px", color: "#8b949e" }}>
                        <span>Experience:</span>
                        <span style={{ color: "#fff", fontWeight: "bold" }}>{character?.experience ?? 0} / 100</span>
                    </div>
                </div>

                <button
                    type="button"
                    onClick={handleClick}
                    style={{
                        padding: "16px",
                        backgroundColor: "#238636",
                        color: "#fff",
                        border: "none",
                        borderRadius: "8px",
                        fontSize: "16px",
                        fontWeight: "bold",
                        cursor: "pointer",
                        boxShadow: "0 4px 12px rgba(35, 134, 54, 0.4)",
                        transition: "all 0.2s",
                        textTransform: "uppercase",
                        letterSpacing: "1px"
                    }}
                >
                    Enter World
                </button>
            </div>
        </main>
    );
}