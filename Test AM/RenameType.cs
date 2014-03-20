namespace Test_AM
{
    /// <summary>
    /// Formas de renombrar.
    /// </summary>
    public enum RenameType
    {
        /// <summary>No modificada el caso.</summary>
        None,
        
        /// <summary>Convierte a mayúsculas.</summary>
        Upper,

        /// <summary>Convierte a minúsculas.</summary>
        Lower,

        /// <summary>La primera letra de cada palabra en mayúsculas.</summary>
        WordUpper,

        /// <summary>La primera letra de la oración en mayúsculas.</summary>
        SentenceUpper,
    }
}
