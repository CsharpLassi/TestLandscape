<Effect>
    <Technique name="Main">
        <Pass name="Pass1">
            <Shader type="PixelShader" filename="modelEffect/main.ps">

            </Shader>
            <Shader type="VertexShader" filename="modelEffect/main.vs">

            </Shader>
            <Attributes>
                <attribute name="position">Position</attribute>
                <attribute name="normal">Normal</attribute>
                <attribute name="color">Color</attribute>
            </Attributes>
        </Pass>
    </Technique>
    
    <Technique name="Transparent">
            <Pass name="Pass1">
                <Shader type="PixelShader" filename="modelEffect/transparent.ps">
    
                </Shader>
                <Shader type="VertexShader" filename="modelEffect/transparent.vs">
                
                </Shader>
                <Attributes>
                    <attribute name="position">Position</attribute>
                </Attributes>
            </Pass>
        </Technique>
    
    <Technique name="Shadow">
        <Pass name="Pass1">
            <Shader type="PixelShader" filename="shadowEffect/shadow.ps">
    
            </Shader>
            <Shader type="VertexShader" filename="shadowEffect/shadow.vs">
    
            </Shader>
            <Attributes>
                <attribute name="position">Position</attribute>
            </Attributes>
        </Pass>
    </Technique>
</Effect>
