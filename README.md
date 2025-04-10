# Innago.Public.NotifictionTemplater | v1

> Version 1.0.0

## Path Table

| Method | Path | Description |
| --- | --- | --- |
| GET | [/generate](#getgenerate) |  |
| POST | [/template](#posttemplate) |  |
| GET | [/generateFromSavedTemplate](#getgeneratefromsavedtemplate) |  |

## Reference Table

| Name | Path | Description |
| --- | --- | --- |
| GenerateFromSavedTemplateInput | [#/components/schemas/GenerateFromSavedTemplateInput](#componentsschemasgeneratefromsavedtemplateinput) |  |
| GenerateInput | [#/components/schemas/GenerateInput](#componentsschemasgenerateinput) |  |
| TemplateSaveInput | [#/components/schemas/TemplateSaveInput](#componentsschemastemplatesaveinput) |  |

## Path Details

***

### [GET]/generate

- Description  
Generates a string from a model and a template

#### RequestBody

- application/json

```ts
{
  template: string
  model: string
}
```

#### Responses

- 200 OK

`text/plain`

```ts
{
  "type": "string"
}
```

***

### [POST]/template

- Description  
Saves a template

#### RequestBody

- application/json

```ts
{
  key: string
  template: string
}
```

#### Responses

- 200 OK

***

### [GET]/generateFromSavedTemplate

- Description  
Generates a string from a model and a saved template

#### RequestBody

- application/json

```ts
{
  key: string
  model: string
}
```

#### Responses

- 200 OK

`text/plain`

```ts
{
  "type": "string"
}
```

## References

### #/components/schemas/GenerateFromSavedTemplateInput

```ts
{
  key: string
  model: string
}
```

### #/components/schemas/GenerateInput

```ts
{
  template: string
  model: string
}
```

### #/components/schemas/TemplateSaveInput

```ts
{
  key: string
  template: string
}
```