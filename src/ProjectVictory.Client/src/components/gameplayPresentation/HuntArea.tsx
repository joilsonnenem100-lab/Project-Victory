import HuntStatus from "./hunt/HuntStatus";
import HuntAction from "./hunt/HuntAction";
import HuntContext from "./hunt/HuntContext";


interface Props
{
    children: React.ReactNode;
}


export default function HuntArea(
    {
        children
    }: Props
)
{
    return (
        <section>


            <HuntStatus>

                {children}

            </HuntStatus>



            <HuntAction>

                {children}

            </HuntAction>



            <HuntContext>

                {children}

            </HuntContext>


        </section>
    );
}