import CombatStatus from "./combat/CombatStatus";
import CombatResult from "./combat/CombatResult";
import CombatFeedback from "./combat/CombatFeedback";


interface Props
{
    children: React.ReactNode;
}


export default function CombatArea(
    {
        children
    }: Props
)
{
    return (
        <section>


            <CombatStatus>

                {children}

            </CombatStatus>



            <CombatResult>

                {children}

            </CombatResult>



            <CombatFeedback>

                {children}

            </CombatFeedback>


        </section>
    );
}