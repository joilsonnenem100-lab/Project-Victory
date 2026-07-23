// src/ProjectVictory.Client/src/pages/HuntPage.tsx

interface CharacterInfo
{
    id?: string;
    name?: string;
    level?: number;
    experience?: number;
    characterClass?: string;
}

interface Props
{
    character?: CharacterInfo;
    onBackToTown: () => void;
    onStartHunt: () => void;
}

export default function HuntPage({ character, onBackToTown, onStartHunt }: Props)
{
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
                flexDirection: "column",
                fontFamily: "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif",
                color: "#e2e8f0",
                overflow: "hidden"
            }}
        >
            <header
                style={{
                    height: "70px",
                    backgroundColor: "#131722",
                    borderBottom: "2px solid #30363d",
                    display: "flex",
                    justifyContent: "space-between",
                    alignItems: "center",
                    padding: "0 32px",
                    boxShadow: "0 4px 12px rgba(0, 0, 0, 0.5)"
                }}
            >
                <h1 style={{ fontSize: "20px", color: "#f0b429", margin: 0, letterSpacing: "1px", textTransform: "uppercase" }}>
                    Project Victory — Hunt Zone
                </h1>
                <div style={{ display: "flex", gap: "20px", fontSize: "14px", color: "#8b949e" }}>
                    <span>Herói: <strong style={{ color: "#fff" }}>{character?.name || "Herói"}</strong></span>
                    <span>Level: <strong style={{ color: "#fff" }}>{character?.level || 1}</strong></span>
                </div>
            </header>

            <div
                style={{
                    flex: 1,
                    display: "flex",
                    justifyContent: "center",
                    alignItems: "center",
                    padding: "24px"
                }}
            >
                <div
                    style={{
                        width: "100%",
                        maxWidth: "700px",
                        padding: "48px",
                        backgroundColor: "#131722",
                        border: "2px solid #30363d",
                        borderRadius: "16px",
                        boxShadow: "0 16px 40px rgba(0, 0, 0, 0.7)",
                        textAlign: "center",
                        display: "flex",
                        flexDirection: "column",
                        gap: "24px"
                    }}
                >
                    <h2 style={{ fontSize: "28px", color: "#f0b429", margin: 0 }}>Zona de Caça (Hunt)</h2>
                    <p style={{ fontSize: "15px", color: "#8b949e", margin: 0, lineHeight: "1.6" }}>
                        Prepare-se para entrar em combate, derrotar criaturas e coletar recursos valiosos.
                    </p>

                    <div style={{ display: "flex", gap: "16px", justifyContent: "center", marginTop: "12px" }}>
                        <button
                            onClick={onStartHunt}
                            style={{
                                padding: "14px 28px",
                                backgroundColor: "#238636",
                                color: "#fff",
                                border: "none",
                                borderRadius: "8px",
                                fontSize: "15px",
                                fontWeight: "bold",
                                cursor: "pointer",
                                boxShadow: "0 4px 12px rgba(35, 134, 54, 0.4)"
                            }}
                        >
                            Iniciar Caçada
                        </button>
                        <button
                            onClick={onBackToTown}
                            style={{
                                padding: "14px 28px",
                                backgroundColor: "transparent",
                                color: "#58a6ff",
                                border: "1px solid #58a6ff",
                                borderRadius: "8px",
                                fontSize: "15px",
                                fontWeight: "bold",
                                cursor: "pointer"
                            }}
                        >
                            Retornar à Cidade
                        </button>
                    </div>
                </div>
            </div>
        </main>
    );
}